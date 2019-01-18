using System.Threading.Tasks;
using LinqToDB;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SunEngine.Commons.DataBase;
using SunEngine.Commons.Models;

namespace SunEngine.Controllers
{
    public class PulseController : BaseController
    {
        private DataBaseConnection db;
        
        public PulseController(UserManager<User> userManager,DataBaseConnection db) : base(userManager)
        {
            this.db = db;
        }
        
        [HttpGet]
        [HttpPost]
        [AllowAnonymous]
        public IActionResult Pulse()
        {
            return Ok(new {SomeDataTest = "SomeData Test!"});
        }
        
        [HttpGet]
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> PulseDb()
        {
            bool anyUserGroup = await  db.UserGroups.AnyAsync();
            return Ok(new {db_UserGroups_AnyAsync = anyUserGroup});
        }
    }
}