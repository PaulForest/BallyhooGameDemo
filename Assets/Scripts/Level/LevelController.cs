using System;
using UnityEngine;
using Util;

namespace Level
{
    [RequireComponent(typeof(DontDestroyOnLoad))]
    public class LevelController : MonoBehaviour
    {
        public static LevelController Instance
        {
            get
            {
                if (_instance) return _instance;

                GameObject go = new GameObject("LevelController");
                _instance = go.AddComponent<LevelController>();

                return _instance;
            }
        }

        private static LevelController _instance;

        private bool _isPlayerWinning = false;

        private void Start()
        {
            _instance = this;

            // ResetAllStaticData.Reset();

            GlobalEvents.LastBallDestroyed.AddListener(OnLastBallDestroyed);
            GlobalEvents.FirstBallInGoal.AddListener(OnFirstBallInGoal);

            // Ensure that this singleton exists, especially when running in-editor in a game scene.
            // var a = GameController.Instance;
        }

        public static void ResetInstance()
        {
            HaltInstance();
            var levelController = LevelController.Instance;
        }

        public static void HaltInstance()
        {
            if (_instance)
            {
                Destroy(Instance.gameObject);
                _instance = null;
            }
        }

        /// <summary>
        /// Stops everything related to gameplay for this level
        /// </summary>
        public void HaltExecution()
        {
        }

        private void OnDestroy()
        {
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
                GlobalEvents.LevelWon?.Invoke();
            }
            else
            {
                GlobalEvents.LevelLost?.Invoke();
            }
        }
    }
}
