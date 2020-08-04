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
    public enum MusicType
    {
        None,
        Title,
        Gameplay,
        Boss
    }

    private AudioLowPassFilter _audioLowPassFilter;

    private AudioSource _audioSource;

    private float _distortionStartingVolume;
    private bool _isDistorting;

    private bool _isTransitioning;

    private MusicType _lastPlayed = MusicType.None;
    [SerializeField] private List<AudioClip> bossMusic;
    [SerializeField] private List<AudioClip> gameplayMusic;
    [SerializeField] private List<AudioClip> titleMusic;

    public float transitionTimeSeconds = 1;

    public static MusicPlayer Instance { get; private set; }

    private void Awake()
    {
        if (Instance && Instance != this)
        {
            DestroyImmediate(gameObject);
            return;
        }

        Instance = this;

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
            PlayBossMusic();
        else
            PlayGameplayMusic();
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
    }

    public void TurnDistortionOff()
    {
        if (!_isDistorting) return;
        _isDistorting = false;

        _audioLowPassFilter.enabled = false;
    }

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
        while (_isTransitioning) yield return null;

        _isTransitioning = true;

        var startingVolume = _audioSource.volume;
        var transitionTimePerFrame = transitionTimeSeconds * Time.deltaTime;
        if (_audioSource.isPlaying)
        {
            for (var t = 0f; t < transitionTimeSeconds; t += transitionTimePerFrame)
                _audioSource.volume = Mathf.Lerp(startingVolume, 0, t);

            _audioSource.Stop();
        }

        _audioSource.clip = clip;
        _audioSource.Play();

        for (var t = 0f; t < transitionTimeSeconds; t += transitionTimePerFrame)
            _audioSource.volume = Mathf.Lerp(0, startingVolume, t);

        _isTransitioning = false;
    }

    private void OnDestroy()
    {
        GlobalEvents.MainMenuShown.RemoveListener(OnMainMenuShown);
        GlobalEvents.LevelStart.RemoveListener(OnLevelStart);
    }
}
