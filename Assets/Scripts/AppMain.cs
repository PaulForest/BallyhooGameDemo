using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Util;

[RequireComponent(typeof(DontDestroyOnLoad))]
public class AppMain : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(StartGame());
    }

    private IEnumerator StartGame()
    {
        yield return SceneManager.LoadSceneAsync("GameplayScene");

        GameController.Instance.StartPlaying();
    }
}
