using UnityEngine;

public class OnlyTouchOnce : MonoBehaviour
{

    /// <summary>
    /// This is a bitfield of all the splitters this ball has made contact with
    /// </summary>
    [HideInInspector] public int mSplittersUsedBitfield;

    private void Awake()
    {
        mSplittersUsedBitfield = 0;
    }

}
