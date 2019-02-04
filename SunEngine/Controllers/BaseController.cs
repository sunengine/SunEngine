using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SunEngine.Authentication;
using SunEngine.Commons.Models;
using SunEngine.Commons.Services;
using SunEngine.Stores;

namespace SunEngine.Controllers
{
    public abstract class BaseController : Controller 
    {
        protected readonly MyUserManager userManager;
        protected readonly IUserGroupStore userGroupStore;
        
        protected BaseController(IUserGroupStore userGroupStore, MyUserManager userManager)
        {
            this.userGroupStore = userGroupStore;
            this.userManager = userManager;
        }
        
        public new MyClaimsPrincipal User
        {
            get
            {
                MyClaimsPrincipal myClaimsPrincipal = base.User as MyClaimsPrincipal;
                return myClaimsPrincipal ?? new MyClaimsPrincipal(base.User,userGroupStore);
            }
        }

        public Task<User> GetUserAsync()
        {
            return userManager.FindByIdAsync(User.UserId.ToString());
        }
    }

    public class ErrorViewModel
    {
        public string ErrorName { get; set; }
        public string ErrorText { get; set; }
    }
    
    public class ErrorsViewModel
    {
        public string[] ErrorsNames { get; set; }
        public string[] ErrorsTexts { get; set; }
    }
}