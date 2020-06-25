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

        private void LevelLost(LevelData levelData)
        {
            // throw new NotImplementedException();
        }

        private void LevelWon(LevelData levelData)
        {
//            throw new NotImplementedException();
        }

        private void LevelStart(LevelData levelData)
        {
            // throw new NotImplementedException();
        }
    }
}
