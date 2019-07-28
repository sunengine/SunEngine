using System.Collections.Concurrent;
using System.Collections.Generic;
using SunEngine.Core.Cache.CacheModels;
using SunEngine.Core.DataBase;

namespace SunEngine.Core.Cache.Services
{
    public class ComponentsCache : ISunMemoryCache
    {
        private readonly object lockObject = new object();

        private readonly IDataBaseFactory dataBaseFactory;
        
        protected ConcurrentDictionary<string, ComponentServerCached> serverComponents;
        protected ConcurrentDictionary<string, ComponentClientCached> clientComponents;

        public ComponentServerCached GetComponentServerCached(string name)
        {
            lock (lockObject)
            {
                if (serverComponents.TryGetValue(name, out ComponentServerCached componentServerCached))
                    return componentServerCached;
            }
        } 
        
        
        public ComponentsCache(IDataBaseFactory dataBaseFactory)
        {
            this.dataBaseFactory = dataBaseFactory;
        }
        
        public void Initialize()
        {

        }

        public void Reset()
        {
            
        }
    }
}
