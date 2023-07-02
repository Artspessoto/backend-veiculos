using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    internal class Cache
    {
        private static readonly ObjectCache cache = MemoryCache.Default;

        public static object get(string key)
        {
            return cache.Get(key);
        }

        public static void add(string key, object obj, int time)
        {
            CacheItemPolicy policy = new CacheItemPolicy();
            policy.AbsoluteExpiration = DateTimeOffset.Now.AddSeconds(time);
            cache.Add(key, obj, policy);
        }

        public static void add(string key, object obj)
        {
            cache.Add(key, obj, null);
        }

        public static void delete(string key)
        {
            cache.Remove(key);
        }
    }
}