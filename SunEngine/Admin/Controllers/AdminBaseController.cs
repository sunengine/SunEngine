using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SunEngine.Controllers;
using SunEngine.Managers;
using SunEngine.Security;
using SunEngine.Stores;

namespace SunEngine.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = RoleNames.Admin)]
    public class AdminBaseController : BaseController
    {
        public AdminBaseController(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }
    }
}