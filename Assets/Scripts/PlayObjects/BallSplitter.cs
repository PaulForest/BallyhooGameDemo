using Balls;
using TMPro;
using UnityEngine;

namespace PlayObjects
{
    [RequireComponent(typeof(CollideOnlyOncePlayerBall), typeof(TMP_Text))]
    public class BallSplitter : MonoBehaviour
    {
        [SerializeField] private CollideOnlyOncePlayerBall collideOnlyOnce;

        [Header("How many balls will this create?")] [SerializeField]
        protected int mSplitCount;

        [SerializeField] private TMP_Text splitCountLabel;

        private void Start()
        {
            collideOnlyOnce = GetComponent<CollideOnlyOncePlayerBall>();
            collideOnlyOnce.onCollisionEvent.AddListener(OnCollisionEvent);

            if (!splitCountLabel) splitCountLabel = GetComponent<TMP_Text>();
            if (!splitCountLabel) splitCountLabel = GetComponentInChildren<TMP_Text>();
            if (!splitCountLabel)
            {
                Debug.LogError("I need a text field set", this);
                return;
            }

            splitCountLabel.text = $"{mSplitCount} X";
        }

        private void OnDestroy()
        {
            collideOnlyOnce.onCollisionEvent.RemoveListener(OnCollisionEvent);
        }

        private void OnCollisionEvent(PlayerBall ball)
        {
            if (!collideOnlyOnce.CanCollideWithBall(ball)) return;

            DoTheSplits(ball);
        }

        /// <summary>
        ///     Clones the <see cref="originalBall" /> <see cref="mSplitCount" /> times.
        ///     We're doing the splits, and there are balls around.  There's a Jean-Claude Van Damme joke in there somewhere. :)
        /// </summary>
        /// <param name="originalBall" />
        private void DoTheSplits(PlayerBall originalBall)
        {
            collideOnlyOnce.SetYouCannotCollideWithMeT(originalBall);
            var bitField = originalBall.mInteractedWithBitField;

            BallSpawnPoint.AddNewInstance(gameObject, transform.position,
                new CollideOnlyOnceData {MyBitFieldMask = bitField},
                mSplitCount, originalBall.transform.localScale.x);

            GlobalEvents.BallSplitEvent?.Invoke(originalBall, this);
        }
    }
}
