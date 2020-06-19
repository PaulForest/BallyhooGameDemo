using System.Collections.Specialized;
using UnityEngine;
using UnityEngine.Events;

namespace Balls
{
    /// <summary>
    /// Provides an efficient way to restrict the number of times collision logic is followed, even when there are many
    /// active objects that could collide with this.
    /// You should use <see cref="CollideOnlyOnce{TOnlyTouchOnce,TUnityEvent}"/> for objects that are relatively few, and <see cref="OnlyTouchOnce"/> for objects that are
    /// relatively numerous.
    /// The idea here is to use a unique, bit mask with exactly one bit set, for each instance of
    /// <see cref="CollideOnlyOnce{TOnlyTouchOnce,TUnityEvent}"/>.  Each instance of <see cref="OnlyTouchOnce"/> keeps an
    /// int that records which instance of <see cref="CollideOnlyOnce{TOnlyTouchOnce,TUnityEvent}"/> is has already
    /// interacted with.  If that int has the bit set, it has done its interaction; if it's cleared, they haven't
    /// interacted.
    /// The idea is to avoid having lists on either of these classes, and allow for hundreds of <see cref="OnlyTouchOnce"/>
    /// instances.
    /// A limitation is that you can only have up to 32 instances of <see cref="CollideOnlyOnce{TOnlyTouchOnce,TUnityEvent}"/>
    /// at any one time.
    /// </summary>
    /// <typeparam name="TOnlyTouchOnce"></typeparam>
    /// <typeparam name="TUnityEvent"></typeparam>
    public abstract class CollideOnlyOnce<TOnlyTouchOnce, TUnityEvent> : MonoBehaviour
        where TOnlyTouchOnce : OnlyTouchOnce
        where TUnityEvent : UnityEvent<TOnlyTouchOnce>, new()
    {
        /// <summary>
        /// Each splitter has exactly one unique bit set.  As long as there are no more that 32 splitters in use, we can
        /// </summary>
        private int _myBitFieldMask;

        private static int _lastBitFieldMask;

        public TUnityEvent onCollisionEvent = new TUnityEvent();

        public static void ResetStaticData()
        {
            _lastBitFieldMask = 0;
        }

        private void Start()
        {
            _myBitFieldMask = BitVector32.CreateMask(_lastBitFieldMask);
            _lastBitFieldMask = _myBitFieldMask;
        }

        private void OnCollisionEnter(Collision other)
        {
            OnTriggerEnter(other.collider);
        }

        private void OnTriggerEnter(Collider other)
        {
            var ball = other.GetComponent<TOnlyTouchOnce>();
            if (!ball) return;

            if (!CanCollideWithBall(ball)) return;

            onCollisionEvent?.Invoke(ball);
        }

        public bool CanCollideWithBall(TOnlyTouchOnce ball)
        {
            return (ball.mInteractedWithBitField & _myBitFieldMask) == 0;
        }

        public void SetCannotCollideWithT(TOnlyTouchOnce ball)
        {
            ball.mInteractedWithBitField |= _myBitFieldMask;
        }
    }
}
