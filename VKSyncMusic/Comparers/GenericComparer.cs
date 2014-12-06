using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VKSyncMusic.Model;

namespace VKSyncMusic.Comparers
{
    public class GenericComparer<T> : IEqualityComparer<T> where T : IDownnloadedData
    {
        public bool Equals(T x, T y)
        {
            return x.IsEqual(y);
           /* bool isNamesEquals = x.FileName.ToLower() == y.FileName.ToLower();
            return isNamesEquals;*/
        }

        public int GetHashCode(T obj)
        {
            return 0;
        }
    }
}
