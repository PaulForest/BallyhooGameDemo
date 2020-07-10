using System.Collections.Generic;
using JetBrains.Annotations;
using UI;
using UnityEngine;

namespace Level
{
    public class LevelView : MonoBehaviour
    {
        [SerializeField] private SimplePrompt simplePromptPrefab;
        [SerializeField] private SimplePromptOption simplePromptOptionPrefab;
        
        public void ClickLevelStartButton()
        {
            GlobalEvents.LevelStart?.Invoke(Player.Instance.LastLevelPlayed);
            levelStartGo.SetActive(false);
        }

        public void ClickLevelWonButton()
        {
            GameController.Instance.StartNextLevel();
        }

        [SerializeField] private GameObject levelStartGo;
        [SerializeField] private GameObject levelLostGo;
        [SerializeField] private GameObject levelWonGo;

        private enum CurrentStateEnum
        {
            [UsedImplicitly] None,
            LevelStart,
            NormalGameplay,
            LevelLost,
            LevelWon
        }

        private CurrentStateEnum CurrentState
        {
            get => _currentState;
            set
            {
                if (_currentState == value)
                {
                    return;
                }

                _currentState = value;

                levelStartGo.SetActive(_currentState == CurrentStateEnum.LevelStart);
                levelLostGo.SetActive(_currentState == CurrentStateEnum.LevelLost);
                levelWonGo.SetActive(_currentState == CurrentStateEnum.LevelWon);
            }
        }

        private CurrentStateEnum _currentState;

        private void Start()
        {
            GlobalEvents.LevelStart.AddListener(LevelStart);
            GlobalEvents.LevelWon.AddListener(LevelWon);
            GlobalEvents.LevelLost.AddListener(LevelLost);
            GlobalEvents.BackButtonPressed.AddListener(OnBackButtonPressed);

            CurrentState = CurrentStateEnum.LevelStart;
        }

        public async void OnBackButtonPressed()
        {
            GlobalEvents.BackButtonPressed.RemoveListener(OnBackButtonPressed); // Re-add this later
            
            var prompt = SimplePrompt.Spawn(simplePromptPrefab, simplePromptOptionPrefab, "Back to Main Menu?",
                new List<SimplePromptOptionData>
                {
                    new SimplePromptOptionData
                    {
                        buttonText = "Resume",
                        simplePromptCallback = OnResumeGame
                    },
                    new SimplePromptOptionData
                    {
                        buttonText = "Quit Level",
                        simplePromptCallback = () => GameController.Instance.BackToMainMenu()
                    }
                }
            );

            await prompt;
        }

        private void OnResumeGame()
        {
            
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
