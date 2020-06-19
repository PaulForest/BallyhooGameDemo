using Balls;
using UnityEngine;
using Random = UnityEngine.Random;

namespace PlayObjects
{
    [RequireComponent(typeof(CollideOnlyOncePlayerBall))]
    public class BallSplitter : MonoBehaviour
    {
        [Header("How many balls will this split into?")] [SerializeField]
        protected int mSplitCount;

        private CollideOnlyOncePlayerBall _collideOnlyOnce;

        private void Start()
        {
            _collideOnlyOnce = GetComponent<CollideOnlyOncePlayerBall>();
            _collideOnlyOnce.onCollisionEvent.AddListener(OnCollisionEvent);
        }

        private void OnDestroy()
        {
            _collideOnlyOnce.onCollisionEvent.RemoveListener(OnCollisionEvent);
        }

        private void OnCollisionEvent(PlayerBall ball)
        {
            DoTheSplits(ball);
        }

        /// <summary>
        /// Clones the <see cref="originalBall"/> <see cref="mSplitCount"/> times.
        /// </summary>
        /// <param name="originalBall"/>
        private void DoTheSplits(PlayerBall originalBall)
        {
            if (!_collideOnlyOnce.CanCollideWithBall(originalBall)) return;

            _collideOnlyOnce.SetCannotCollideWithT(originalBall);

            for (var i = 0; i < mSplitCount; i++)
            {
                var pos = Random.onUnitSphere;

                var transform1 = originalBall.transform;
                pos *= transform1.localScale.x;
                pos += transform1.position;
                pos.z = 0;

                var go = GameObject.Instantiate(originalBall.gameObject, pos, transform1.rotation);
                var newBall = go.GetComponent<PlayerBall>();
                _collideOnlyOnce.SetCannotCollideWithT(newBall);
            }

            GlobalEvents.BallSplitEvent?.Invoke(originalBall, this);
        }
    }
}
