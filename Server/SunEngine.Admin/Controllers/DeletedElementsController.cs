using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SunEngine.Admin.Services;

namespace SunEngine.Admin.Controllers
{
	public class DeletedElementsController : BaseAdminController
	{
		private readonly ICleanerManager cleanerManager;

		public DeletedElementsController(IServiceProvider serviceProvider,
			ICleanerManager cleanerManager) : base(serviceProvider)
		{
			this.cleanerManager = cleanerManager;
		}

		[HttpPost]
		public async Task<IActionResult> DeleteAllMarkedMaterials()
		{
			var deleted = await cleanerManager.DeleteAllMarkedMaterials();
			return Ok(deleted);
		}
	}
}