using System.Threading.Tasks;

namespace SunEngine.Stores
{   
    public interface IMemoryCache
    {
        void Initialize();
        Task InitializeAsync();
        void Reset();
    }
}