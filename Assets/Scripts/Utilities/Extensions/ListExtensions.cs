using System;
using System.Collections.Generic;

namespace Utilities
{
    public static partial class Extensions
    {
        private static List<int> hashCodeList;
        public static bool SortBy<T>(this List<T> list, Comparison<T> comparison)
        {
            hashCodeList ??= new List<int>();
            foreach (var item in list)
            {
                hashCodeList.Add(item.GetHashCode());
            }
            list.Sort(comparison);
            for (var i = 0; i < list.Count; i++)
            {
                if (hashCodeList[i] != list[i].GetHashCode())
                {
                    return true;
                }
            }
            return false;
        }
    }
}