using System.Threading.Tasks;

namespace SunEngine.Stores
{   
    public interface IMemoryStore
    {
        void Initialize();
        Task InitializeAsync();
        void Reset();
    }
}