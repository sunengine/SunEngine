using System.Threading.Tasks;

namespace SunEngine.Commons.Cache
{   
    public interface ISunMemoryCache
    {
        /// <summary>
        /// Initialize (get) cache from database
        /// </summary>
        void Initialize();
        
        /// <summary>
        /// Initialize (get) cache from database, async verison
        /// </summary>
        Task InitializeAsync();
        
        /// <summary>
        /// Reset to null all cache items and collections, to re obtain them on the next request
        /// </summary>
        void Reset();
    }
}