using UnityEngine;
using Util;

[RequireComponent(typeof(DontDestroyOnLoad))]
public class MainMenu : MonoBehaviour
{
    private void Start()
    {
        GlobalEvents.MainMenuShown?.Invoke();
    }

    public void StartGame()
    {
        GameController.Instance.StartGame();
    }
}
