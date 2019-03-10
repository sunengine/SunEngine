using System.Threading.Tasks;

namespace SunEngine.Cache
{   
    public interface IMemoryCache
    {
        void Initialize();
        Task InitializeAsync();
        void Reset();
    }
}