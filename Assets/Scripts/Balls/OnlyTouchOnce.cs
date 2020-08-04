using UnityEngine;

namespace Balls
{
    public class OnlyTouchOnce : MonoBehaviour, IResettableNonStaticData
    {
        /// <summary>
        ///     This is a bitfield of all the instances of <see cref="CollideOnlyOnce{TOnlyTouchOnce,TUnityEvent}" /> this
        ///     instance has interacted with.
        /// </summary>
        public int mInteractedWithBitField;

        public void ResetNonStaticData()
        {
            mInteractedWithBitField = 0;
        }

        private void Awake()
        {
            mInteractedWithBitField = 0;
        }

        protected void ResetOnlyTouchOnceData()
        {
            mInteractedWithBitField = 0;
        }

        public CollideOnlyOnceData GetCollideOnlyOnceData()
        {
            return new CollideOnlyOnceData
            {
                MyBitFieldMask = mInteractedWithBitField
            };
        }

        public void UpdateCollideOnlyOnceDataFromExistingData(CollideOnlyOnceData collideOnlyOnceData)
        {
            mInteractedWithBitField |= collideOnlyOnceData.MyBitFieldMask;
        }
    }
}