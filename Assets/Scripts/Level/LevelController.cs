using System;
using Balls;
using UnityEngine;

namespace Level
{
    public class LevelController : MonoBehaviour
    {
        public static LevelController Instance
        {
            get
            {
                if (_instance) return _instance;

                var go = new GameObject("LevelController");
                _instance = go.AddComponent<LevelController>();

                return _instance;
            }
        }

        private static LevelController _instance;

        private bool _isPlayerWinning;
        private LevelData CurrentLevelData { get; }

        private void Start()
        {
            _instance = this;

            GlobalEvents.LastBallDestroyed.AddListener(OnLastBallDestroyed);
            GlobalEvents.FirstBallInGoal.AddListener(OnFirstBallInGoal);
        }

        /// <summary>
        /// Stops everything related to gameplay for this level
        /// </summary>
        public void HaltExecution()
        {
            BallPool.Instance.ResetData();
        }

        private void OnDestroy()
        {
            _instance = null;

            GlobalEvents.LastBallDestroyed.RemoveListener(OnLastBallDestroyed);
            GlobalEvents.FirstBallInGoal.RemoveListener(OnFirstBallInGoal);
        }

        private void OnFirstBallInGoal()
        {
            _isPlayerWinning = true;
        }

        private void OnLastBallDestroyed()
        {
            if (_isPlayerWinning)
            {
                GlobalEvents.LevelWon?.Invoke(CurrentLevelData);
            }
            else
            {
                GlobalEvents.LevelLost?.Invoke(CurrentLevelData);
            }
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                
            }
        }
    }
}
