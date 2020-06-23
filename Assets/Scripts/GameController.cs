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
        GlobalEvents.LevelWon.AddListener(OnLevelWon);
        GlobalEvents.LevelLost.AddListener(OnLevelLost);

        StartLevel(Player.Instance.LastLevelPlayed);
    }

    private void StartLevel(int levelBuildIndex)
    {
        StartCoroutine(StartLevelCoroutine(levelBuildIndex));
    }

    private IEnumerator StartLevelCoroutine(int levelBuildIndex)
    {
        ResetAllStaticData.Reset();


        yield return SceneManager.LoadSceneAsync(levelBuildIndex, LoadSceneMode.Additive);

        var player = Player.Instance;

        LevelController.ResetInstance();

        GlobalEvents.LevelStart?.Invoke();
    }

    private void OnLevelWon()
    {
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
        ResetAllStaticData.Reset();
        LevelController.Instance.HaltExecution();


        yield return new WaitForSeconds(1);

        yield return SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().buildIndex);

        StartLevel(Player.Instance.LastLevelPlayed);
    }
}
