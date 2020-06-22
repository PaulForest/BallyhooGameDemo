using UnityEngine;

namespace Balls
{
    public class BitMaskCollider
    {
        public static void ResetStaticData()
        {
            Debug.Log($"ResetStaticData(): old value was: {lastBitFieldMask}");

            lastBitFieldMask = 0;
        }

        /// <summary>
        /// Each splitter has exactly one unique bit set.  As long as there are no more that 32 splitters in use, we can
        /// </summary>
        public static int lastBitFieldMask = 0;
    }
}
