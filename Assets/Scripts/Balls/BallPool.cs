using Level;
using UnityEngine;

namespace Balls
{
    public class BallPool : ObjectPool<PlayerBall>
    {
        public static BallPool Instance
        {
            get
            {
                if (_instance) return _instance;

                var go = new GameObject("BallPool");
                _instance = go.AddComponent<BallPool>();
                return _instance;
            }
        }

        private static BallPool _instance;

        protected override void Awake()
        {
            if (_instance && _instance != this)
            {
                DestroyImmediate(this);
                return;
            }

            base.Awake();
            _instance = this;
            
            GlobalEvents.LevelStart.AddListener(OnLevelChanged);
            GlobalEvents.LevelWon.AddListener(OnLevelChanged);
            GlobalEvents.LevelLost.AddListener(OnLevelChanged);
        }

        private void OnLevelChanged(LevelData arg0)
        {
            ResetData();
        }

        private void OnDestroy()
        {
            GlobalEvents.LevelStart.RemoveListener(OnLevelChanged);
            GlobalEvents.LevelWon.RemoveListener(OnLevelChanged);
            GlobalEvents.LevelLost.RemoveListener(OnLevelChanged);

            foreach (var go in pool)
            {
                Destroy(go);
            }

            pool.Clear();
        }
    }
}