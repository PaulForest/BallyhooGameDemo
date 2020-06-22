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


            Debug.LogError($"GameController should be in the scene");
            return null;

            // var go = new GameObject("GameController");
            // _instance = go.AddComponent<GameController>();
            // return _instance;
        }
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
        StartLevel(Player.Instance.LastLevelPlayed);

        GlobalEvents.LevelWon.AddListener(OnLevelWon);
        GlobalEvents.LevelLost.AddListener(OnLevelLost);
    }

    private void StartLevel(int levelBuildIndex)
    {
        Debug.Log($"GameController.StartLevel(): levelBuildIndex={levelBuildIndex}");
        SceneManager.LoadScene(levelBuildIndex, LoadSceneMode.Additive);

        ResetAllStaticData.Reset();

        var player = Player.Instance;

        LevelController.ResetInstance();

        GlobalEvents.LevelStart?.Invoke();
    }

    private void OnLevelWon()
    {
        Debug.Log("GameController.OnLevelWon");
        StartCoroutine(OnLevelWonCoroutine());
    }

    private IEnumerator OnLevelWonCoroutine()
    {
        ResetAllStaticData.Reset();
        LevelController.Instance.HaltExecution();

        var player = Player.Instance;

        yield return SceneManager.UnloadSceneAsync(player.LastLevelPlayed);

        player.LastLevelPlayed = LevelList.Instance.GetNextLevelBuildIndex(player.LastLevelPlayed);
        player.SaveData();

        StartLevel(player.LastLevelPlayed);
    }

    private void OnLevelLost()
    {
        Debug.Log("GameController.OnLevelLost()");
        StartCoroutine(OnLevelLostCoroutine());
    }

    private IEnumerator OnLevelLostCoroutine()
    {
        yield return null;

        ResetAllStaticData.Reset();
        LevelController.Instance.HaltExecution();

        StartLevel(Player.Instance.LastLevelPlayed);
    }
}
