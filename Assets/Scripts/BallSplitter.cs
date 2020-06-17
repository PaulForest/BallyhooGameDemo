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

        Debug.Log($"myBitFieldMask={_myBitFieldMask}");
    }

    private void OnTriggerEnter(Collider other)
    {
        PlayerBall ball = other.GetComponent<PlayerBall>();
        if (!ball) return;

        if (!CanCollideWithBall(ball)) return;

        DoTheSplits(ball);
    }

    private void DoTheSplits(PlayerBall ball)
    {
        SetCannotCollideWithPlayerBall(ball);

        for (int i = 0; i < mSplitCount; i++)
        {
            GameObject go = GameObject.Instantiate(ball.gameObject);
            SetCannotCollideWithPlayerBall(go.GetComponent<PlayerBall>());
        }
    }

    public bool CanCollideWithBall(PlayerBall ball)
    {
        return (ball.mSplittersUsedBitfield & _myBitFieldMask) == 0;
    }

    private void SetCannotCollideWithPlayerBall(PlayerBall ball)
    {
        ball.mSplittersUsedBitfield |= _myBitFieldMask;
    }


    // Update is called once per frame
    void Update()
    {
    }
}