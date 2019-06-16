using System;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using SunEngine.Core.Presenters;

namespace SunEngine.Core.Controllers
{
    /// <summary>
    /// Send all categories information controller
    /// </summary>
    public class CategoriesController : BaseController
    {
        protected readonly ICategoriesPresenter categoriesPresenter;

        private readonly JsonSerializerSettings jsonSerializerSettings = new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore,
            ContractResolver =  new CamelCasePropertyNamesContractResolver()
        };


        public CategoriesController(
            ICategoriesPresenter categoriesPresenter,
            IServiceProvider serviceProvider) : base(serviceProvider)
        {
            this.categoriesPresenter = categoriesPresenter;
        }

        [HttpPost]
        [HttpGet] // HttpGet - For pulse and testing 
        public virtual IActionResult GetAllCategoriesAndAccesses()
        {
            var rez = categoriesPresenter.GetRootCategoryInfoWithAccesses(User.Roles);
            return Json(rez, jsonSerializerSettings);
        }
    }
}