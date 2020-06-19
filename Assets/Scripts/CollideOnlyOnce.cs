using System;
using System.Collections.Specialized;
using UnityEngine;

public class CollideOnlyOnce : MonoBehaviour, ResettableStaticData
{
    /// <summary>
    /// Each splitter has exactly one unique bit set.  As long as there are no more that 32 splitters in use, we can
    /// </summary>
    private int _myBitFieldMask;

    private static int _lastBitFieldMask;

    public readonly UnityPlayerBallEvent onCollisionEvent = new UnityPlayerBallEvent();

    public void ResetStaticData()
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
        var ball = other.GetComponent<PlayerBall>();
        if (!ball) return;

        if (!CanCollideWithBall(ball)) return;

        onCollisionEvent?.Invoke(ball);
    }

    public bool CanCollideWithBall(PlayerBall ball)
    {
        return (ball.mSplittersUsedBitfield & _myBitFieldMask) == 0;
    }

    public void SetCannotCollideWithPlayerBall(PlayerBall ball)
    {
        ball.mSplittersUsedBitfield |= _myBitFieldMask;
    }
}
