using System.Threading.Tasks;
using LinqToDB;
using SunEngine.Core.DataBase;
using SunEngine.Core.Models;
using SunEngine.Core.Services;

namespace SunEngine.Admin.Managers
{
    public interface IComponentsAdminManager
    {
        Task CreateComponent(Component component);
    }

    public class ComponentsAdminManager : DbService, IComponentsAdminManager
    {
        public ComponentsAdminManager(DataBaseConnection db) : base(db)
        {
        }
        
        public Task CreateComponent(Component component)
        {
            return db.InsertAsync(component);
        }
    }
}
