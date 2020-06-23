using UnityEngine;

namespace Level
{
    public class LevelView : MonoBehaviour
    {
        private void Start()
        {
            GlobalEvents.LevelStart.AddListener(LevelStart);
            GlobalEvents.LevelWon.AddListener(LevelWon);
            GlobalEvents.LevelLost.AddListener(LevelLost);
        }

        private void OnDestroy()
        {
            GlobalEvents.LevelStart.RemoveListener(LevelStart);
            GlobalEvents.LevelWon.RemoveListener(LevelWon);
            GlobalEvents.LevelLost.RemoveListener(LevelLost);
        }

        private void LevelLost()
        {
            // throw new NotImplementedException();
        }

        private void LevelWon()
        {
//            throw new NotImplementedException();
        }

        private void LevelStart()
        {
            // throw new NotImplementedException();
        }
    }
}
