using System;
using System.Threading.Tasks;
using LinqToDB;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SunEngine.Commons.DataBase;
using SunEngine.Commons.Misc;

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
            bool anyUserGroup = await db.Roles.AnyAsync();
            return Ok(new {db_UserGroups_AnyAsync = anyUserGroup});
        }

        [HttpGet]
        [HttpPost]
        [AllowAnonymous]
        public virtual IActionResult TestException()
        {
            throw new Exception("TestException");
        }

        [HttpGet]
        [HttpPost]
        [AllowAnonymous]
        public virtual IActionResult TestError1()
        {
            return BadRequest(new ErrorView("Code", "Desc", "Message"));
        }

        [HttpGet]
        [HttpPost]
        [AllowAnonymous]
        public virtual IActionResult TestError2()
        {
            try
            {
                throw new Exception("Message");
            }
            catch(Exception e)
            {
                return BadRequest(new ErrorView("Code", "Desc", e));
            }
        }
    }
}