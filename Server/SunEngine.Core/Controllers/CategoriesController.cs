using System;
using Microsoft.AspNetCore.Mvc;
using SunEngine.Core.Presenters;

namespace SunEngine.Core.Controllers
{
	/// <summary>
	/// Send all categories information controller
	/// </summary>
	public class CategoriesController : BaseController
	{
		protected readonly ICategoriesPresenter categoriesPresenter;

		public CategoriesController(
			ICategoriesPresenter categoriesPresenter,
			IServiceProvider serviceProvider) : base(serviceProvider)
		{
			this.categoriesPresenter = categoriesPresenter;
		}

		[HttpPost]
		public virtual IActionResult GetAllCategoriesAndAccesses()
		{
			var rez = categoriesPresenter.GetRootCategoryInfoWithAccesses(User.Roles);
			return Json(rez);
		}
	}
}