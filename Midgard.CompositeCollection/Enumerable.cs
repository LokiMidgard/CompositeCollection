using System;
using System.Collections.Generic;

using System.Text;
using System.Threading.Tasks;

namespace Midgard.CompositeCollection
{
    static class Enumerable
    {

        public static int IndexOf<T>(this IEnumerable<T> e, T item)
        {
            var l = e as IList<T>;
            if (l != null)
                return l.IndexOf(item);
            var index = 0;
            foreach (var i in e)
            {
                if (object.Equals(i, item))
                    return index;
                index++;
            }
            return -1;
        }

    }
}
