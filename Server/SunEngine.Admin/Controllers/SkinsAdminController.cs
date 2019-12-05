using System;
using System.IO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SunEngine.Admin.Services;
using SunEngine.Core.Utils;

namespace SunEngine.Admin.Controllers
{
    public class SkinsAdminController : BaseAdminController
    {
        private readonly SkinsAdminService skinsAdminService;

        public SkinsAdminController(
            SkinsAdminService skinsAdminService,
            IServiceProvider serviceProvider) : base(serviceProvider)
        {
            this.skinsAdminService = skinsAdminService;
        }

        [HttpPost]
        public IActionResult UploadSkin(IFormFile file)
        {
            skinsAdminService.UploadSkin(file);

            return Ok();
        }

        [HttpPost]
        public IActionResult DeleteSkin(string name)
        {
            skinsAdminService.DeleteSkin(name);

            return Ok();
        }

        [HttpPost]
        public IActionResult ChangeSkin(string name)
        {
            skinsAdminService.ChangeSkin(name);

            return Ok();
        }

        [HttpPost]
        public IActionResult GetAllSkins()
        {
            var allSkins = skinsAdminService.GetAllSkins();

            return Json(allSkins);
        }
    }
}
