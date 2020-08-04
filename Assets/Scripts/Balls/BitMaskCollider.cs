namespace Balls
{
    public class BitMaskCollider
    {
        /// <summary>
        ///     Each splitter has exactly one unique bit set.  As long as there are no more that 32 splitters in use, we can
        /// </summary>
        public static int lastBitFieldMask;

        public static void ResetStaticData()
        {
            lastBitFieldMask = 0;
        }
    }
}