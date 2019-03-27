using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SunEngine.Commons.Controllers;
using SunEngine.Commons.Security;

namespace SunEngine.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = RoleNames.Admin)]
    public abstract class BaseAdminController : BaseController
    {
        protected BaseAdminController(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }
    }
}