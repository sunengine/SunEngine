using System;
using System.Diagnostics;
using System.Reflection;
using System.Threading.Tasks;
using LinqToDB;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SunEngine.Core.DataBase;

namespace SunEngine.Core.Controllers
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
        public virtual async ValueTask<IActionResult> PulseDb()
        {
            bool anyRole = await db.Roles.AnyAsync();
            return Ok(new {db_Roles_AnyAsync = anyRole});
        }
        
        [HttpGet]
        [HttpPost]
        [AllowAnonymous]
        public virtual IActionResult Version()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            FileVersionInfo fileVersionInfo = FileVersionInfo.GetVersionInfo(assembly.Location);
            string version = fileVersionInfo.ProductVersion;
            return Ok(new {version});
        }
    }
}
