using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SunEngine.Core.Filters;
using SunEngine.Core.Presenters;

namespace SunEngine.Core.Controllers
{
	public class SearchController : BaseController
	{
		protected readonly ISearchPresenter searchPresenter;

		public SearchController(
			ISearchPresenter searchPresenter,
			IServiceProvider serviceProvider) : base(serviceProvider)
		{
			this.searchPresenter = searchPresenter;
		}

		// TODO : Add check max and min searchPattern length
		[HttpPost]
		[IpAndUserSpamProtectionFilter(IpTimeoutSeconds = 15, UserTimeoutSeconds = 10)]
		public async Task<IActionResult> SearchUsers(string searchString)
		{
			if (string.IsNullOrEmpty(searchString) || searchString.Length < 3 || searchString.Length > 20)
				return BadRequest();

			var users = await searchPresenter.SearchByUsernameOrLink(searchString);

			return Ok(users);
		}

		[HttpPost]
		[IpAndUserSpamProtectionFilter(IpTimeoutSeconds = 15, UserTimeoutSeconds = 10)]
		public async Task<IActionResult> SearchMaterials(string searchString)
		{
			if (string.IsNullOrEmpty(searchString) || searchString.Length < 3 || searchString.Length > 20)
				return BadRequest();

			return Ok(await searchPresenter.SearchByMaterials(searchString));
		}
	}
}