using System.Collections;
using Level;
using UnityEngine;
using UnityEngine.SceneManagement;
using Util;

[RequireComponent((typeof(DontDestroyOnLoad)))]
public class GameController : MonoBehaviour
{
    public static GameController Instance
    {
        get
        {
            if (_instance) return _instance;


            // Debug.LogError($"GameController should be in the scene");
            // return null;

            var go = new GameObject("GameController");
            _instance = go.AddComponent<GameController>();
            return _instance;
        }
    }

    public void StartNextLevel()
    {
        StartLevel(Player.Instance.LastLevelPlayed);
    }

    private static GameController _instance;

    private void Awake()
    {
        if (_instance && _instance != this)
        {
            DestroyImmediate(gameObject);
            return;
        }

        _instance = this;
        ResetAllStaticData.Reset();
    }

    private void Start()
    {
        GlobalEvents.LevelWon.AddListener(OnLevelWon);
        GlobalEvents.LevelLost.AddListener(OnLevelLost);
    }

    public void StartGame()
    {
        StartLevel(Player.Instance.LastLevelPlayed);
    }

    private void StartLevel(LevelData levelData)
    {
        StartCoroutine(StartLevelCoroutine(levelData));
    }

    private IEnumerator StartLevelCoroutine(LevelData levelData)
    {
        ResetAllStaticData.Reset();

        yield return SceneManager.LoadSceneAsync(levelData.buildIndex);

        var player = Player.Instance;
        var levelController = LevelController.Instance;

        GlobalEvents.BeforeLevelStart?.Invoke(levelData);
    }

    public void BackToMainMenu()
    {
        LevelController.Instance.HaltExecution();
        ResetAllStaticData.Reset();

        SceneManager.LoadScene(0);

        Time.timeScale = 1;
    }


    private void OnLevelWon(LevelData levelData)
    {
        ResetAllStaticData.Reset();
        LevelController.Instance.HaltExecution();

        var player = Player.Instance;

        player.LastLevelPlayed = LevelList.Instance.GetNextLevelData();
        player.SaveData();
    }

    private void OnLevelLost(LevelData levelData)
    {
        ResetAllStaticData.Reset();
        LevelController.Instance.HaltExecution();
    }

    private void OnDestroy()
    {
        GlobalEvents.LevelWon.RemoveListener(OnLevelWon);
        GlobalEvents.LevelLost.RemoveListener(OnLevelLost);
    }
}