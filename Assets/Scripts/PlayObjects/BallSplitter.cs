using Balls;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

namespace PlayObjects
{
    [RequireComponent(typeof(CollideOnlyOncePlayerBall), typeof(TMP_Text))]
    public class BallSplitter : MonoBehaviour
    {
        [Header("How many balls will this split into?")] [SerializeField]
        protected int mSplitCount;

        [SerializeField] private CollideOnlyOncePlayerBall _collideOnlyOnce;
        [SerializeField] private TMP_Text _splitCountLabel;

        /// <summary>
        ///
        /// </summary>
        private int _ballsLeftToSpawn = 0;

        private void Start()
        {
            _collideOnlyOnce = GetComponent<CollideOnlyOncePlayerBall>();
            _collideOnlyOnce.onCollisionEvent.AddListener(OnCollisionEvent);

            if (!_splitCountLabel) _splitCountLabel = GetComponent<TMP_Text>();
            if (!_splitCountLabel) _splitCountLabel = GetComponentInChildren<TMP_Text>();
            if (!_splitCountLabel)
            {
                Debug.LogError("I need a text field set", this);
                return;
            }

            _splitCountLabel.text = $"{mSplitCount} X";
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
        /// We're doing the splits, and there are balls around.  There's a Jean-Claude Van Damme joke in there somewhere. :)
        /// </summary>
        /// <param name="originalBall"/>
        private void DoTheSplits(PlayerBall originalBall)
        {
            if (!_collideOnlyOnce.CanCollideWithBall(originalBall)) return;

            _collideOnlyOnce.SetCannotCollideWithT(originalBall);

            var transform1 = originalBall.transform;
            var radius = transform1.localScale.x;
            var layerMask = 1 >> LayerMask.NameToLayer("Default");

            for (var i = 0; i < mSplitCount; i++)
            {
                var pos = new Vector3();
                const int maxIteration = 10;
                int j;
                for (j = 0; j < maxIteration; j++)
                {
                    pos = Random.onUnitSphere;
                    pos.z = 0;
                    pos *= radius;
                    pos += transform1.position;
                    
                    if (!Physics.CheckSphere(pos, radius, layerMask))
                    {
                        break;
                    }
                }

                if (j >= maxIteration)
                {
                    Debug.LogWarning($"Too many iterations: {j}, max {maxIteration}", this);
                }

                var go = GameObject.Instantiate(originalBall.gameObject, pos, transform1.rotation);
                var newBall = go.GetComponent<PlayerBall>();
                _collideOnlyOnce.SetCannotCollideWithT(newBall);
            }

            GlobalEvents.BallSplitEvent?.Invoke(originalBall, this);
        }
    }
}
