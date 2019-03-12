using System.Threading.Tasks;

namespace SunEngine.Cache
{   
    public interface IMyMemoryCache
    {
        void Initialize();
        Task InitializeAsync();
        void Reset();
    }
}