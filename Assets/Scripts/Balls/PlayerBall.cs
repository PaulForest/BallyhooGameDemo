using PhysSound;
using UnityEngine;

namespace Balls
{
    [RequireComponent(typeof(PhysSoundObject))]
    public class PlayerBall : OnlyTouchOnce, IPoolableObject
    {
        private PhysSoundObject _physSoundObject;
        private bool _recentlyReset;

        private void Awake()
        {
            _physSoundObject = GetComponent<PhysSoundObject>();
            if (!_physSoundObject)
            {
                Debug.LogError($"{this}: I need a PhysSoundObject component", this);
            }
        }

        private void OnEnable()
        {
            ResetOnlyTouchOnceData();
            _physSoundObject.SetEnabled(true);
        }

        private void OnDisable()
        {
            GlobalEvents.BallDestroyed?.Invoke(this);

            if (!BallPool.Instance.HasBallsInPlay)
            {
                GlobalEvents.LastBallDestroyed?.Invoke();
            }

            _physSoundObject.SetEnabled(false);
        }

        public void BeforeReset()
        {
            _recentlyReset = true;
        }

        public void AfterReset()
        {
            _recentlyReset = false;
        }
    }
}