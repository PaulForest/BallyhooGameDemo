using UnityEngine;
using UnityEngine.SceneManagement;
using Util;

[RequireComponent(typeof(DontDestroyOnLoad))]
public class AppMain : MonoBehaviour
{
    private void Start()
    {
        StartGame();
    }

    private void StartGame()
    {
        SceneManager.LoadScene("GameplayScene");
    }
}
