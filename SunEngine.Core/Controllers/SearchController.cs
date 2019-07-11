using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
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

        [HttpPost]
        public async Task<IActionResult> SearchUsers(string searchString)
        {
            if (string.IsNullOrEmpty(searchString))
            {
                return BadRequest();
            }

            var users = await searchPresenter.SearchByUsernameAndLinkAsync(searchString);
            if (users.Length == 0)
            {
                return NotFound("Users not found");
            }
            return Ok(users);
        }
    }
}