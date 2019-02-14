using System.Threading.Tasks;
using LinqToDB;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SunEngine.DataBase;
using SunEngine.Managers;
using SunEngine.Stores;

namespace SunEngine.Controllers
{
    public class PulseController : BaseController
    {
        protected readonly DataBaseConnection db;
        
        public PulseController(
            MyUserManager userManager,
            DataBaseConnection db,
            IUserGroupStore userGroupStore) : base(userGroupStore, userManager)
        {
            this.db = db;
        }
        
        [HttpGet]
        [HttpPost]
        [AllowAnonymous]
        public virtual IActionResult Pulse()
        {
            return Ok(new {SomeDataTest = "SomeData Test!"});
        }
        
        [HttpGet]
        [HttpPost]
        [AllowAnonymous]
        public virtual async Task<IActionResult> PulseDb()
        {
            bool anyUserGroup = await  db.Roles.AnyAsync();
            return Ok(new {db_UserGroups_AnyAsync = anyUserGroup});
        }
    }
}