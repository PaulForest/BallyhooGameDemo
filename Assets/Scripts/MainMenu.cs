using UI;
using UnityEngine;
using Util;

[RequireComponent(typeof(DontDestroyOnLoad))]
public class MainMenu : MonoBehaviour
{
    private void Start()
    {
        GlobalEvents.MainMenuShown?.Invoke();
        GlobalEvents.BackButtonPressed.AddListener(OnBackButtonPressed);
    }

    private void OnBackButtonPressed()
    {
        SimplePrompt.Spawn();

    }

    public void StartGame()
    {
        GameController.Instance.StartGame();
    }
}
