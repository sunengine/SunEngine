using System.Threading.Tasks;

namespace SunEngine.Core.Cache.Services
{   
    public interface ISunMemoryCache
    {
        /// <summary>
        /// Initialize (get) cache from database
        /// </summary>
        void Initialize();
        
        /// <summary>
        /// Initialize (get) cache from database, async version
        /// </summary>
        Task InitializeAsync();
        
        /// <summary>
        /// Reset to null all cache items and collections, to re obtain them on the next request
        /// </summary>
        void Reset();
    }
}
