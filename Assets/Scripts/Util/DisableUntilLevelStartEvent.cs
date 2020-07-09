using Level;
using UnityEngine;

namespace Util
{
    public class DisableUntilLevelStartEvent : MonoBehaviour
    {
        private void Awake()
        {
            gameObject.SetActive(false);

            GlobalEvents.LevelStart.AddListener(OnLevelStart);
        }

        private void OnLevelStart(LevelData arg0)
        {
            gameObject.SetActive(true);
        }

        private void OnDestroy()
        {
            GlobalEvents.LevelStart.RemoveListener(OnLevelStart);
        }
    }
}