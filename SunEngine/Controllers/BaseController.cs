using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SunEngine.Managers;
using SunEngine.Models;
using SunEngine.Security.Authentication;
using SunEngine.Stores;

namespace SunEngine.Controllers
{
    public abstract class BaseController : Controller 
    {
        protected readonly MyUserManager userManager;
        protected readonly IRolesCache RolesCache;
        
        protected BaseController(IRolesCache rolesCache, MyUserManager userManager)
        {
            this.RolesCache = rolesCache;
            this.userManager = userManager;
        }

        private MyClaimsPrincipal _user;
        
        public new MyClaimsPrincipal User
        {
            get
            {
                if (_user == null)
                {
                    MyClaimsPrincipal myClaimsPrincipal = base.User as MyClaimsPrincipal;
                    _user = myClaimsPrincipal ?? new MyClaimsPrincipal(base.User,RolesCache);
                }

                return _user;
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
        public string[] ErrorsNames { get; set; }
        public string[] ErrorsTexts { get; set; }
    }    
}