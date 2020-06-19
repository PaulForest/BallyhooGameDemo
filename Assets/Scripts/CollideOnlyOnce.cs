using System.Collections.Specialized;
using UnityEngine;
using UnityEngine.Events;

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
        onCollisionEvent = new TUnityEvent();

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
        return (ball.mSplittersUsedBitfield & _myBitFieldMask) == 0;
    }

    public void SetCannotCollideWithT(TOnlyTouchOnce ball)
    {
        ball.mSplittersUsedBitfield |= _myBitFieldMask;
    }
}
