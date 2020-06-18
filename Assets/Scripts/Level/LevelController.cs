using UnityEngine;

namespace Level
{
    public class LevelController : MonoBehaviour
    {
        private void Start()
        {
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
        }

        private void OnLastBallDestroyed()
        {
        }
    }
}
