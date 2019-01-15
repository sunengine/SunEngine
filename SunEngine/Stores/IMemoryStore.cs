using System.Threading.Tasks;

namespace SunEngine.Stores
{
    public interface IMemoryStore<TEntity> where TEntity : class
    {
        void InitializeOrReset();
        Task InitializeOrResetAsync();
    }
    
    public interface IMemoryStore
    {
        void InitializeOrReset();
        Task InitializeOrResetAsync();
    }
}