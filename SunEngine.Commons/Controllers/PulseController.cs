using System;
using System.Threading.Tasks;
using LinqToDB;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SunEngine.Commons.DataBase;

namespace SunEngine.Commons.Controllers
{
    /// <summary>
    /// For API testing only
    /// </summary>
    public class PulseController : BaseController
    {
        protected readonly DataBaseConnection db;

        public PulseController(
            DataBaseConnection db,
            IServiceProvider serviceProvider) : base(serviceProvider)
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
            bool anyRole = await db.Roles.AnyAsync();
            return Ok(new {db_Roles_AnyAsync = anyRole});
        }
    }
}