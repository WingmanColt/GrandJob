using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HireMe.Core.Extensions
{
    public static class AllResultsAsync
    {
      /*  public static async Task<IList<T>> AllResultsToAsync<T>(this IAsyncEnumerable<T> source)
        {
            if (null == source)
                throw new ArgumentNullException(nameof(source));


            var list = new List<T>();
            await foreach (var t in source)
            {
                list.Add(t);
            }

            return list;
        }
        public static async Task<IAsyncEnumerable<string>> AllResultsToAsync2(this IAsyncEnumerable<string> source)
        {
            if (null == source)
                throw new ArgumentNullException(nameof(source));

            var list = new List<string>();
           // source = source.ToAsyncEnumerable();

            await foreach (var t in source)
            {
                list.Add(t);
            }

            return list.ToAsyncEnumerable();
        }
      */
    }
}
