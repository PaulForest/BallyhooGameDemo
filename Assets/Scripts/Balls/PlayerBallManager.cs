using Level;
using UnityEngine;

namespace Balls
{
    /// <summary>
    /// Handles events of interest to Player.  Specifically, when the level starts, reset the pool of balls.
    /// Runs automatically, as long as <see cref="Instance"/> is referred before <see cref="GlobalEvents.LevelStart"/>
    /// is dispatched.
    /// </summary>
    public class PlayerBallManager : MonoBehaviour
    {
        private static PlayerBallManager _instance;

        public static PlayerBallManager Instance
        {
            get
            {
                if (null != _instance) return _instance;

                var go = new GameObject("PlayerBallManager");
                _instance = go.AddComponent<PlayerBallManager>();
                return _instance;
            }
        }

        private void Awake()
        {
            GlobalEvents.LevelStart.AddListener(OnLevelStart);
        }

        private void OnDestroy()
        {
            GlobalEvents.LevelStart.RemoveListener(OnLevelStart);
        }

        private void OnLevelStart(LevelData arg0)
        {
            BallPool.Instance.ResetData();
        }
    }
}
