using System.Collections.Concurrent;
using System.Linq;
using SunEngine.Security.Filters;

namespace SunEngine.Stores
{
    public class SpamProtectionCache
    {
        protected ConcurrentDictionary<string, RequestFree> Requests { get; } =
            new ConcurrentDictionary<string, RequestFree>();

        protected int cycle = 0;

        private void NextTick()
        {
            if (++cycle > 100)
            {
                RemoveExpired();
                cycle = 0;
            }
        }

        public RequestFree Find(string key)
        {
            NextTick();
            Requests.TryGetValue(key, out var requestFree);
            return requestFree;
        }

        public bool Add(string key, RequestFree requestFree)
        {
            return Requests.TryAdd(key, requestFree);
        }

        public void RemoveExpired()
        {
            var removeList = Requests.Where(x => !x.Value.Working()).Select(x => x.Key).ToList();
            removeList.ForEach(x => Requests.TryRemove(x, out _));
        }
    }
}