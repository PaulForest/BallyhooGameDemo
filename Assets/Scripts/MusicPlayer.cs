using System;
using System.Collections;
using System.Collections.Generic;
using Level;
using UnityEngine;
using Util;
using Random = UnityEngine.Random;

[RequireComponent(typeof(DontDestroyOnLoad), typeof(AudioSource), typeof(AudioLowPassFilter))]
public class MusicPlayer : MonoBehaviour
{
    [SerializeField] private List<AudioClip> titleMusic;
    [SerializeField] private List<AudioClip> gameplayMusic;
    [SerializeField] private List<AudioClip> bossMusic;

    private AudioSource _audioSource;
    private AudioLowPassFilter _audioLowPassFilter;

    private bool _isTransitioning;
    private bool _isDistorting;

    public float transitionTimeSeconds = 1;

    // public float distortionTimeSeconds = 1;
    // public float distortionLowPassCutoffFrequency = 5000;
    // public float distortionLowVolume = 0.5f;

    // private float _startingLowPassCutoffFrequency;
    private float _distortionStartingVolume;

    public static MusicPlayer Instance => _instance;
    private static MusicPlayer _instance;

    public enum MusicType
    {
        None,
        Title,
        Gameplay,
        Boss
    }

    private MusicType _lastPlayed = MusicType.None;

    private void Awake()
    {
        if (_instance && _instance != this)
        {
            DestroyImmediate(gameObject);
            return;
        }

        _instance = this;

        _audioSource = GetComponent<AudioSource>();
        if (!_audioSource) Debug.LogError("I need an AudioSource attached to me", this);

        _audioLowPassFilter = GetComponent<AudioLowPassFilter>();
        if (!_audioLowPassFilter) Debug.LogError("I need an AudioLowPassFilter attached to me", this);
        _audioLowPassFilter.enabled = false;

        if (gameplayMusic.Count == 0) Debug.LogError("I need gameplayMusic assigned to me", this);
        if (titleMusic.Count == 0) Debug.LogError("I need titleMusic assigned to me", this);
        if (bossMusic.Count == 0) Debug.LogError("I need bossMusic assigned to me", this);

        GlobalEvents.MainMenuShown.AddListener(OnMainMenuShown);
        GlobalEvents.LevelStart.AddListener(OnLevelStart);
        GlobalEvents.LevelLost.AddListener(OnLevelLost);
    }

    private void OnLevelLost(LevelData arg0)
    {
        TurnDistortionOn();
    }

    private void OnMainMenuShown()
    {
        TurnDistortionOff();

        PlayTitleMusic();
    }

    private void OnLevelStart(LevelData levelData)
    {
        TurnDistortionOff();

        if (levelData.isBoss)
        {
            PlayBossMusic();
        }
        else
        {
            PlayGameplayMusic();
        }
    }

    public void PlayTitleMusic()
    {
        PlayMusic(MusicType.Title);
    }

    public void PlayGameplayMusic()
    {
        PlayMusic(MusicType.Gameplay);
    }

    public void PlayBossMusic()
    {
        PlayMusic(MusicType.Boss);
    }

    public void TurnDistortionOn()
    {
        if (_isDistorting) return;
        _isDistorting = true;

        _audioLowPassFilter.enabled = true;

        // StartCoroutine(TurnDistortionOnCo());
    }

    // private IEnumerator TurnDistortionOnCo()
    // {
    //     // _startingLowPassCutoffFrequency = _audioLowPassFilter.cutoffFrequency;
    //     _distortionStartingVolume = _audioSource.volume;
    //     _audioLowPassFilter.enabled = true;
    //
    //     for (var t = transitionTimeSeconds; t >= 0; t -= transitionTimeSeconds / Time.deltaTime)
    //     {
    //         var factor = t / Time.deltaTime;
    //         // _audioLowPassFilter.cutoffFrequency = _startingLowPassCutoffFrequency * factor;
    //         _audioSource.volume = _distortionStartingVolume * factor;
    //
    //         yield return null;
    //     }
    //
    //     _isDistorting = false;
    // }


    public void TurnDistortionOff()
    {
        if (!_isDistorting) return;
        _isDistorting = false;

        _audioLowPassFilter.enabled = false;

        // StartCoroutine(TurnDistortionOffCo());
    }

    // private IEnumerator TurnDistortionOffCo()
    // {
    //     // var startingLowPassCutoffFrequency = _audioLowPassFilter.cutoffFrequency;
    //     // var startingVolume = _audioSource.volume;
    //
    //
    //     for (var t = 0f; t <= transitionTimeSeconds; t += transitionTimeSeconds / Time.deltaTime)
    //     {
    //         var factor = t / Time.deltaTime;
    //         // _audioLowPassFilter.cutoffFrequency = _startingLowPassCutoffFrequency * factor;
    //         _audioSource.volume = _distortionStartingVolume * factor;
    //
    //         yield return null;
    //     }
    //
    //     _isDistorting = false;
    //     _audioLowPassFilter.enabled = false;
    // }

    private void PlayMusic(MusicType musicType)
    {
        if (_lastPlayed == musicType) return;
        _lastPlayed = musicType;

        List<AudioClip> clips;
        switch (musicType)
        {
            case MusicType.Title:
                clips = titleMusic;
                break;
            case MusicType.Gameplay:
                clips = gameplayMusic;
                break;
            case MusicType.Boss:
                clips = bossMusic;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(musicType), musicType, null);
        }

        if (null == clips || clips.Count == 0)
        {
            Debug.LogError("clips need to be defined", this);
            return;
        }

        var clip = clips[Random.Range(0, clips.Count - 1)];
        StartCoroutine(PlayMusicCo(clip));
    }

    private IEnumerator PlayMusicCo(AudioClip clip)
    {
        while (_isTransitioning)
        {
            yield return null;
        }

        _isTransitioning = true;

        var startingVolume = _audioSource.volume;
        var transitionTimePerFrame = transitionTimeSeconds * Time.deltaTime;
        if (_audioSource.isPlaying)
        {
            for (var t = 0f; t < transitionTimeSeconds; t += transitionTimePerFrame)
            {
                _audioSource.volume = Mathf.Lerp(startingVolume, 0, t);
            }

            _audioSource.Stop();
        }

        _audioSource.clip = clip;
        _audioSource.Play();

        for (var t = 0f; t < transitionTimeSeconds; t += transitionTimePerFrame)
        {
            _audioSource.volume = Mathf.Lerp(0, startingVolume, t);
        }

        _isTransitioning = false;
    }

    private void OnDestroy()
    {
        GlobalEvents.MainMenuShown.RemoveListener(OnMainMenuShown);
        GlobalEvents.LevelStart.RemoveListener(OnLevelStart);
    }
}
