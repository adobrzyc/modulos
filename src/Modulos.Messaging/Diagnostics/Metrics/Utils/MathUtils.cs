using System.Collections.Generic;
using System.Linq;

namespace Modulos.Messaging.Diagnostics.Metrics.Utils
{
    internal static class MathUtils
    {
        internal static decimal CalculateMedian(IEnumerable<decimal> values)
        {
            var copyForSorting = values as IList<decimal> ?? values.ToList();
            if (!copyForSorting.Any()) 
                return 0;
         
            var size = copyForSorting.Count;
            var mid = size/2;
            var median = size%2 != 0 ? copyForSorting[mid] : (copyForSorting[mid] + copyForSorting[mid - 1])/2;
            return median;
        }
    }
}