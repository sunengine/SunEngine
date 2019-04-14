using System.Threading.Tasks;

namespace SunEngine.Commons.Cache
{   
    public interface ISunMemoryCache
    {
        void Initialize();
        Task InitializeAsync();
        void Reset();
    }
}