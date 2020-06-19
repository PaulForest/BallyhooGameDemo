using System;
using UnityEngine;

public class PlayerBall : MonoBehaviour
{
    /// <summary>
    /// This is a bitfield of all the splitters this ball has made contact with
    /// </summary>
    [HideInInspector] public int mSplittersUsedBitfield;

    private void Awake()
    {
        mSplittersUsedBitfield = 0;
        NumberOfNumberOfBallsInPlay++;
    }

    private void OnDestroy()
    {
        NumberOfNumberOfBallsInPlay--;
        GlobalEvents.BallDestroyed?.Invoke(this);

        if (!HasBallsInPlay)
        {
            GlobalEvents.LastBallDestroyed?.Invoke();
        }
    }

    public static bool HasBallsInPlay => NumberOfNumberOfBallsInPlay > 0;
    public static int NumberOfNumberOfBallsInPlay { get; private set; }
}
