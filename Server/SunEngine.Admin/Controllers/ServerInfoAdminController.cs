using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc;
using SunEngine.Core.Services;

namespace SunEngine.Admin.Controllers
{
	public sealed class ServerInfoAdminController : BaseAdminController
	{
		private readonly string ServerInfoJsonFilePath;

		public ServerInfoAdminController(
			IPathService pathService,
			IServiceProvider serviceProvider) : base(serviceProvider)
		{
			ServerInfoJsonFilePath = Path.Combine(pathService.ConfigDir, PathNames.ServerInfoJsonFileName);
		}

		[HttpPost]
		public IActionResult Version()
		{
			Assembly assembly = Assembly.GetAssembly(typeof(SunEngine.Core.Models.Category));
			FileVersionInfo fileVersionInfo = FileVersionInfo.GetVersionInfo(assembly.Location);
			string version = fileVersionInfo.ProductVersion;
			return Ok(new {version});
		}

		[HttpPost]
		public IActionResult DotnetVersion()
		{
			Regex reg = new Regex("v(.*)?$");
			string verToken = reg.Match(AppContext.TargetFrameworkName).Value;
			return Ok(verToken);
		}

		[HttpPost]
		public IActionResult GetServerInfo()
		{
			return new FileStreamResult(System.IO.File.OpenRead(ServerInfoJsonFilePath), "application/json");
		}
	}
}