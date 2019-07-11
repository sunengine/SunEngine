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
        
        public SearchController(ISearchPresenter searchPresenter ,IServiceProvider serviceProvider) : base(serviceProvider)
        {
            this.searchPresenter = searchPresenter;
        }

        [IpSpamProtectionFilter]
        [HttpPost]
        public async Task<IActionResult> SearchUsers(string searchString)
        {
            if (string.IsNullOrEmpty(searchString)) return BadRequest();

            return Ok(await searchPresenter.SearchByUsernameAndLink(searchString));
        }
    }
}