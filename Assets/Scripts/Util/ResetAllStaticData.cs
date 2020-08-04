using Balls;

namespace Util
{
    public static class ResetAllStaticData
    {
        /// <summary>
        ///     Reset anything that has static data
        /// </summary>
        public static void Reset()
        {
            BitMaskCollider.ResetStaticData();
            BallPool.Instance.ResetData();
        }

        // private static Type[] _implementationsOfIResettableStaticData;
        //
        // public static void Reset()
        // {
        //     if (_implementationsOfIResettableStaticData == null)
        //     {
        //         _implementationsOfIResettableStaticData = typeof(IResettableStaticData).FindInterfaces(Filter, null);
        //     }
        //
        //     foreach (var t in _implementationsOfIResettableStaticData)
        //     {
        //TODO  BOO! C# can't have static methods in interfaces until .Net 8, so we can't guarantee that ResetStaticData() is present on these classes
        //         t.ResetStaticData();
        //     }
        // }
        //
        // private static bool Filter(Type m, object filterCriteria)
        // {
        //     return true;
        // }
    }
}