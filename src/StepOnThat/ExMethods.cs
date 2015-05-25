using System.Collections.Generic;
using System.Linq;

namespace StepOnThat
{
    public static class ExMethods
    {
        public static bool IsNullOrEmpty<T>(this IEnumerable<T> coll)
        {
            return coll == null || !coll.Any();
        }
    }
}