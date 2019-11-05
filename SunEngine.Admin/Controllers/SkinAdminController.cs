using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace SunEngine.Admin.Controllers
{
    public class SkinAdminController : BaseAdminController
    {
        public SkinAdminController(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        public IActionResult UploadSkin(IFormFile file)
        {
            
        }
    }
}