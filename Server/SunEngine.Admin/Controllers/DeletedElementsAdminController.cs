using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SunEngine.Admin.Managers;
using SunEngine.Admin.Services;

namespace SunEngine.Admin.Controllers
{
	public class DeletedElementsAdminController : BaseAdminController
	{
		private readonly ICleanerAdminManager cleanerAdminManager;

		public DeletedElementsAdminController(IServiceProvider serviceProvider,
			ICleanerAdminManager cleanerAdminManager) : base(serviceProvider)
		{
			this.cleanerAdminManager = cleanerAdminManager;
		}

		[HttpPost]
		public async Task<IActionResult> DeleteAllMarkedMaterials()
		{
			var deleted = await cleanerAdminManager.DeleteAllMarkedMaterials();
			return Ok(deleted);
		}
	}
}