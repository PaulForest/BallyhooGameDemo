using System;
using UnityEngine;

public class PlayerBall : MonoBehaviour
{
    [HideInInspector] public int mSplittersUsedBitfield;

    private void OnDestroy()
    {
        GlobalEvents.BallDestroyed?.Invoke(this);
    }
}
