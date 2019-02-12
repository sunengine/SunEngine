using Microsoft.AspNetCore.Mvc;
using SunEngine.Managers;
using SunEngine.Presenters;
using SunEngine.Stores;

namespace SunEngine.Controllers
{
    public class CategoriesController : BaseController
    {
        protected readonly ICategoriesPresenter categoriesPresenter;

        public CategoriesController(
            IUserGroupStore userGroupStore,
            ICategoriesPresenter categoriesPresenter,
            MyUserManager userManager) : base(userGroupStore, userManager)
        {
            this.categoriesPresenter = categoriesPresenter;
        }

        [HttpPost]
        [HttpGet] // HttpGet - For pulse and testing 
        public virtual CategoryInfoWithAccesses GetAllCategoriesAndAccesses()
        {
            var rez = categoriesPresenter.CategoryInfoWithAccessesFromCategory(User.UserGroups);
            return rez;
        }
    }
}