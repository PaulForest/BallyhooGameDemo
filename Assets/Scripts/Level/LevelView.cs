using UnityEngine;

namespace Level
{
    public class LevelView : MonoBehaviour
    {
        public enum CurrentStateEnum
        {
            None,
            LevelStart,
            NormalGameplay,
            LevelLost,
            LevelWon,
            BackMenuOpen
        }

        private CurrentStateEnum _currentState;
        [SerializeField] private GameObject backMenuGo;
        [SerializeField] private GameObject levelLostGo;

        [SerializeField] private GameObject levelStartGo;
        [SerializeField] private GameObject levelWonGo;

        public CurrentStateEnum CurrentState
        {
            get => _currentState;
            set
            {
                if (_currentState == value) return;

                _currentState = value;

                levelStartGo.SetActive(_currentState == CurrentStateEnum.LevelStart);
                levelLostGo.SetActive(_currentState == CurrentStateEnum.LevelLost);
                levelWonGo.SetActive(_currentState == CurrentStateEnum.LevelWon);
                backMenuGo.SetActive(_currentState == CurrentStateEnum.BackMenuOpen);
            }
        }

        public void ClickLevelStartButton()
        {
            GlobalEvents.LevelStart?.Invoke(Player.Instance.LastLevelPlayed);
            levelStartGo.SetActive(false);
        }

        public void ClickLevelWonButton()
        {
            GameController.Instance.StartNextLevel();
        }

        private void Start()
        {
            GlobalEvents.LevelStart.AddListener(LevelStart);
            GlobalEvents.LevelWon.AddListener(LevelWon);
            GlobalEvents.LevelLost.AddListener(LevelLost);
            GlobalEvents.BackButtonPressed.AddListener(OnBackButtonPressed);

            CurrentState = CurrentStateEnum.LevelStart;
        }

        public void OnBackButtonPressed()
        {
            switch (CurrentState)
            {
                case CurrentStateEnum.BackMenuOpen:
                    CurrentState = CurrentStateEnum.NormalGameplay;
                    Time.timeScale = 1;
                    break;
                case CurrentStateEnum.NormalGameplay:
                    CurrentState = CurrentStateEnum.BackMenuOpen;
                    Time.timeScale = 0;
                    break;
            }
        }

        public void OnClickQuitButton()
        {
            GameController.Instance.BackToMainMenu();
        }

        private void OnDestroy()
        {
            GlobalEvents.LevelStart.RemoveListener(LevelStart);
            GlobalEvents.LevelWon.RemoveListener(LevelWon);
            GlobalEvents.LevelLost.RemoveListener(LevelLost);
        }

        private void LevelLost(LevelData levelData)
        {
            levelLostGo.SetActive(true);
        }

        private void LevelWon(LevelData levelData)
        {
            levelWonGo.SetActive(true);
        }

        private void LevelStart(LevelData levelData)
        {
            levelStartGo.SetActive(true);
        }
    }
}
