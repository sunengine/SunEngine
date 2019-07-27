using System.Collections.Concurrent;
using System.Linq;
using SunEngine.Core.Filters;

namespace SunEngine.Core.Cache.Services
{
    /// <summary>
    /// Cache to track and prevent to quick posting content on site
    /// </summary>
    public class SpamProtectionCache
    {
        protected ConcurrentDictionary<string, RequestFree> Requests { get; } =
            new ConcurrentDictionary<string, RequestFree>();

        protected int cycle = 0;

        public RequestFree Find(string key)
        {
            Requests.TryGetValue(key, out var requestFree);
            return requestFree;
        }
        
        public bool HasWorkingKey(string key)
        {
            return Requests.TryGetValue(key, out var requestFree) && requestFree.Working();
        }

        public bool Add(string key, RequestFree requestFree)
        {
            return Requests.TryAdd(key, requestFree);
        }
        
        public void AddOrUpdate(string key, RequestFree requestFree)
        {
            if (!Requests.TryAdd(key, requestFree))
                Requests[key] = requestFree;
        }

        public void RemoveExpired()
        {
            var removeList = Requests.Where(x => !x.Value.Working()).Select(x => x.Key).ToList();
            removeList.ForEach(x => Requests.TryRemove(x, out _));
        }
    }
}
