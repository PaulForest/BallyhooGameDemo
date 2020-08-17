using UnityEngine;

namespace Balls
{
    public class NormalPoolableObject : MonoBehaviour, IPoolableObject, IResettableNonStaticData
    {
        /// <summary>
        /// Guards against dispatching the
        /// </summary>
        public bool RecentlyReset { get; private set; }

        /// <summary>
        /// Used by <see cref="IPoolableObject"/> before resetting this instance.
        /// </summary>
        public void BeforeReset()
        {
            RecentlyReset = true;
        }

        /// <summary>
        /// Used by <see cref="IPoolableObject"/> after resetting this instance.
        /// </summary>
        public void AfterReset()
        {
            RecentlyReset = false;
        }

        public void ResetNonStaticData()
        {

        }
    }
}
