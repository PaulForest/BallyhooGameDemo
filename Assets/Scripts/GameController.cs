using System.Collections;
using Level;
using UnityEngine;
using UnityEngine.SceneManagement;
using Util;

[RequireComponent((typeof(DontDestroyOnLoad)))]
public class GameController : MonoBehaviour
{
    public Player Player { get; private set; }

    public static GameController Instance => _instance;
    private static GameController _instance;

    private LevelController _levelController;

    public void StartPlaying()
    {

    }

    private void Awake()
    {
        if (_instance)
        {
            DestroyImmediate(gameObject);
            return;
        }

        _instance = this;
    }

    private void Start()
    {
        Player = Player.Instance;
        if (string.IsNullOrEmpty(Player.LastLevelPlayed))
        {
            Player.LastLevelPlayed = LevelList.Instance.GetFirstLevelName();
        }

        GlobalEvents.LevelWon.AddListener(OnLevelWon);
        GlobalEvents.LevelLost.AddListener(OnLevelLost);
    }

    private void OnLevelWon()
    {
        Debug.Log("GameController.OnLevelWon");
        StartCoroutine(OnLevelWonCoroutine());
    }

    private IEnumerator OnLevelWonCoroutine()
    {
        var oldLevelName = Player.LastLevelPlayed;
        var newLevelName = LevelList.Instance.GetNextLevelName(Player.LastLevelPlayed);

        yield return SceneManager.UnloadSceneAsync(oldLevelName);
        yield return SceneManager.LoadSceneAsync(newLevelName);
    }

    private void OnLevelLost()
    {
        Debug.Log("GameController.OnLevelLost()");
    }
}
