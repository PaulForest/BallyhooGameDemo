using Level;
using UnityEngine;

namespace Balls
{
    public class PlayerBallManager : MonoBehaviour
    {
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

        public bool RecentlyReset => _recentlyReset;
        
        private static PlayerBallManager _instance;
        private bool _recentlyReset = true;


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
            _recentlyReset = true;
        }
    }
}