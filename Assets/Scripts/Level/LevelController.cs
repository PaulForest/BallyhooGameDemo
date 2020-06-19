using UnityEngine;
using Util;

namespace Level
{
    public class LevelController : MonoBehaviour
    {
        private bool _isPlayerWinning = false;

        private void Start()
        {
            ResetAllStaticData.Reset();

            GlobalEvents.LastBallDestroyed.AddListener(OnLastBallDestroyed);
            GlobalEvents.FirstBallInGoal.AddListener(OnFirstBallInGoal);
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
