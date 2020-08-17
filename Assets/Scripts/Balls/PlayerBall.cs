using PhysSound;
using UnityEngine;

namespace Balls
{
    /// <summary>
    /// An object that bounces around in the level.
    /// <list type="bullet">
    /// <item><description>Can correctly guarantee that it only triggers collision logic once when collides with
    /// particular objects for the first time only via <see cref="OnlyTouchOnce"/>.</description></item>
    /// <item><description>Instances are pooled via <see cref="NormalPoolableObject"/>.</description></item>
    /// <item><description>Uses the Physics Object library to play sounds when colliding with objects in the level via
    /// <see cref="PhysSoundObject"/></description></item>
    /// </list>
    /// </summary>
    [RequireComponent(typeof(OnlyTouchOnce))]
    [RequireComponent(typeof(PhysSoundObject))]
    [RequireComponent(typeof(CollideOnlyOncePlayerBall))]
    public class PlayerBall : NormalPoolableObject
    {
        public CollideOnlyOncePlayerBall MyCollideOnlyOncePlayerBall { get; private set; }

        private PhysSoundObject _physSoundObject;
        private NormalPoolableObject _normalPoolableObject;

        private void Awake()
        {
            _physSoundObject = GetComponent<PhysSoundObject>();
            if (!_physSoundObject)
            {
                Debug.LogError($"{this}: I need a PhysSoundObject component", this);
            }

            _normalPoolableObject = GetComponent<NormalPoolableObject>();
            if (!_normalPoolableObject)
            {
                Debug.LogError($"{this}: I need a NormalPoolableObject component", this);
            }

            MyCollideOnlyOncePlayerBall = GetComponent<CollideOnlyOncePlayerBall>();
            if (!MyCollideOnlyOncePlayerBall)
            {
                Debug.LogError($"{this}: I need an OnlyTouchOnce component", this);
            }
        }

        private void OnEnable()
        {
            MyCollideOnlyOncePlayerBall.ResetOnlyTouchOnceData();
            _physSoundObject.SetEnabled(true);
        }

        private void OnDisable()
        {
            _physSoundObject.SetEnabled(false);

            // skip gameplay logic if we're just resetting this instance.
            if (_normalPoolableObject.RecentlyReset) return;

            GlobalEvents.BallDestroyed?.Invoke(this);
        }
    }
}
