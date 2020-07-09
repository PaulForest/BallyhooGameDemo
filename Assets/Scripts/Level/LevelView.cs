using UnityEngine;

namespace Level
{
    public class LevelView : MonoBehaviour
    {
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

        private void Start()
        {
            GlobalEvents.LevelStart.AddListener(LevelStart);
            GlobalEvents.LevelWon.AddListener(LevelWon);
            GlobalEvents.LevelLost.AddListener(LevelLost);

            levelStartGo.SetActive(true);
            levelLostGo.SetActive(false);
            levelWonGo.SetActive(false);
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