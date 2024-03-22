using System.Collections.Generic;

namespace Utilities
{
    public static partial class Extensions
    {
        public static T GetFirst<T>(this HashSet<T> hashSet)
        {
            foreach (var item in hashSet)
            {
                return item;
            }
            return default;
        }
    }
}