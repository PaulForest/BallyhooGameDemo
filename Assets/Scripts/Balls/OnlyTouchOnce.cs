using UnityEngine;

namespace Balls
{
    public class OnlyTouchOnce : MonoBehaviour
    {
        /// <summary>
        /// This is a bitfield of all the instances of <see cref="CollideOnlyOnce{TOnlyTouchOnce,TUnityEvent}"/> this
        /// instance has interacted with.
        /// </summary>
        public int mInteractedWithBitField;

        private void Awake()
        {
            mInteractedWithBitField = 0;
        }

        protected void ResetOnlyTouchOnceData()
        {
            mInteractedWithBitField = 0;
        }
    }
}
