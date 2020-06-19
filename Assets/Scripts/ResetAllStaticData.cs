using System;
using System.Reflection;

public class ResetAllStaticData
{
    public static void Reset()
    {
        CollideOnlyOncePlayerBall.ResetStaticData();
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
    //         t.ResetStaticData();
    //     }
    // }
    //
    // private static bool Filter(Type m, object filterCriteria)
    // {
    //     return true;
    // }
}
