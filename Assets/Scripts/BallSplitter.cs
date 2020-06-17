using System.Collections.Specialized;
using UnityEngine;

public class BallSplitter : MonoBehaviour
{
    [Header("How many balls will this split into?")] [SerializeField]
    protected int mSplitCount;

    private int _myBitFieldMask;

    private static int _lastBitFieldMask;

    private void Start()
    {
        _myBitFieldMask = BitVector32.CreateMask(_lastBitFieldMask);
        _lastBitFieldMask = _myBitFieldMask;
    }

    private void OnTriggerEnter(Collider other)
    {
        var ball = other.GetComponent<PlayerBall>();
        if (!ball) return;

        if (!CanCollideWithBall(ball)) return;

        DoTheSplits(ball);
    }

    /// <summary>
    /// Clones the <see cref="originalBall"/> <see cref="mSplitCount"/> times.
    /// </summary>
    /// <param name="originalBall"/>
    private void DoTheSplits(PlayerBall originalBall)
    {
        if (!CanCollideWithBall(originalBall)) return;

        SetCannotCollideWithPlayerBall(originalBall);

        for (var i = 0; i < mSplitCount; i++)
        {
            var pos = Random.onUnitSphere;

            var transform1 = originalBall.transform;
            pos *= transform1.localScale.x;
            pos += transform1.position;
            pos.z = 0;

            var go = GameObject.Instantiate<PlayerBall>(originalBall, pos, transform1.rotation);
            SetCannotCollideWithPlayerBall(go);
        }

        GlobalEvents.BallSplitEvent?.Invoke(originalBall, this);
    }

    private bool CanCollideWithBall(PlayerBall ball)
    {
        return (ball.mSplittersUsedBitfield & _myBitFieldMask) == 0;
    }

    private void SetCannotCollideWithPlayerBall(PlayerBall ball)
    {
        ball.mSplittersUsedBitfield |= _myBitFieldMask;
    }
}
