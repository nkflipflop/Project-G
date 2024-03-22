using System;

namespace Utilities
{
    public static partial class Extensions
    {
        public static bool Contains<T>(this Enum e, T flag)
        {
            var intValue = (int)(object)e;
            var intFlag = (int)(object)flag;
            return (intValue & intFlag) != 0;
        }
    }
}