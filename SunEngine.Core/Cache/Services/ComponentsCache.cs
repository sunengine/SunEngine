using System;
using System.Collections.Generic;
using System.Linq;
using SunEngine.Core.Cache.CacheModels;
using SunEngine.Core.DataBase;

namespace SunEngine.Core.Cache.Services
{
    public interface IComponentsCache : ISunMemoryCache
    {
        ComponentServerCached GetComponentServerCached(string name);
    }

    public class ComponentsCache : IComponentsCache
    {
        private readonly object lockObject = new object();

        private readonly IDataBaseFactory dataBaseFactory;

        protected Dictionary<string, ComponentServerCached> serverComponents;
        protected Dictionary<string, ComponentClientCached> clientComponents;

        protected Dictionary<string, ComponentServerCached> ServerComponents
        {
            get
            {
                lock (lockObject)
                {
                    if (serverComponents == null)
                        Initialize();
                    return serverComponents;
                }
            }
        }

        protected Dictionary<string, ComponentClientCached> ClientComponents
        {
            get
            {
                lock (lockObject)
                {
                    if (clientComponents == null)
                        Initialize();
                    return clientComponents;
                }
            }
        }

        public ComponentServerCached GetComponentServerCached(string name)
        {
            lock (lockObject)
            {
                return ServerComponents.TryGetValue(name, out ComponentServerCached componentServerCached)
                    ? componentServerCached
                    : null;
            }
        }


        public ComponentsCache(IDataBaseFactory dataBaseFactory)
        {
            this.dataBaseFactory = dataBaseFactory;
        }

        public void Initialize()
        {
            lock (lockObject)
            {
                using (var db = dataBaseFactory.CreateDb())
                {
                    var components = db.Components.ToList();
                    
                    serverComponents = components.ToDictionary(x => x.Name,
                        x => new ComponentServerCached(x, Type.GetType(x.Type + "ComponentData"))
                    );
                    clientComponents = components.ToDictionary(x => x.Name, x => new ComponentClientCached(x));
                }
            }
        }

        public void Reset()
        {
            lock (lockObject)
            {
                serverComponents = null;
                clientComponents = null;
            }
        }
    }
}
