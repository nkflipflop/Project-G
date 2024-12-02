using System;

namespace Utilities
{
    public static partial class Extensions
    {
        public static bool Contains<T>(this Enum e, T flag) 
        {
            var enumValueAsInt = Convert.ToInt64(e);
            var flagAsInt = Convert.ToInt64(flag);
            return (enumValueAsInt & flagAsInt) == flagAsInt;
        }
    }
}