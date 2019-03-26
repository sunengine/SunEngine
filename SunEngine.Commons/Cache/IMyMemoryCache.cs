using System.Threading.Tasks;

namespace SunEngine.Commons.Cache
{   
    public interface IMyMemoryCache
    {
        void Initialize();
        Task InitializeAsync();
        void Reset();
    }
}