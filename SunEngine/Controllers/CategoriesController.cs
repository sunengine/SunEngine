using System;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using SunEngine.Managers;
using SunEngine.Presenters;
using SunEngine.Stores;

namespace SunEngine.Controllers
{
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
            var rez = categoriesPresenter.CategoryInfoWithAccessesFromCategory(User.Roles);
            return Json(rez, jsonSerializerSettings);
        }
    }
}