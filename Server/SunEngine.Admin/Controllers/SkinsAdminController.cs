using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SunEngine.Admin.Services;
using SunEngine.Core.Cache.Services;

namespace SunEngine.Admin.Controllers
{
	public class SkinsAdminController : BaseAdminController
	{
		private readonly SkinsAdminService skinsAdminService;
		private readonly IDynamicConfigCache dynamicConfigCache;

		public SkinsAdminController(
			SkinsAdminService skinsAdminService,
			IDynamicConfigCache dynamicConfigCache,
			IServiceProvider serviceProvider) : base(serviceProvider)
		{
			this.dynamicConfigCache = dynamicConfigCache;
			this.skinsAdminService = skinsAdminService;
		}

		[HttpPost]
		public IActionResult UploadSkin(IFormFile file)
		{
			skinsAdminService.UploadSkin(file, SkinsAdminService.SkinType.Main);

			return Ok();
		}

		public IActionResult EnablePartialSkin(string name, bool enable)
		{
			skinsAdminService.EnablePartialSkin(name, enable);
			dynamicConfigCache.Initialize();

			return Ok();
		}

		[HttpPost]
		public IActionResult UploadPartialSkin(IFormFile file)
		{
			skinsAdminService.UploadSkin(file, SkinsAdminService.SkinType.Partial);

			return Ok();
		}

		[HttpPost]
		public IActionResult DeleteSkin(string name)
		{
			skinsAdminService.DeleteSkin(name, SkinsAdminService.SkinType.Main);

			return Ok();
		}

		[HttpPost]
		public IActionResult DeletePartialSkin(string name)
		{
			skinsAdminService.DeleteSkin(name, SkinsAdminService.SkinType.Partial);

			return Ok();
		}

		[HttpPost]
		public IActionResult ChangeSkin(string name)
		{
			skinsAdminService.ChangeSkin(name);
			dynamicConfigCache.Initialize();

			return Ok();
		}

		[HttpPost]
		public IActionResult GetAllSkins()
		{
			var allSkins = skinsAdminService.GetAllSkins(SkinsAdminService.SkinType.Main);

			return Json(allSkins);
		}

		[HttpPost]
		public IActionResult GetAllPartialSkins()
		{
			var allSkins = skinsAdminService.GetAllSkins(SkinsAdminService.SkinType.Partial);

			return Json(allSkins);
		}
		
		[HttpPost]
		public IActionResult GetCustomCss()
		{
			var cssText = skinsAdminService.GetCustomCss();

			return Ok(cssText);
		}

		[HttpPost]
		public IActionResult UpdateCustomCss(string cssText)
		{
			skinsAdminService.UpdateCustomCss(cssText);

			return Ok();
		}
	}
}