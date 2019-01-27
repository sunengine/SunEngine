using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SunEngine.Commons.Models;
using SunEngine.Commons.Services;
using SunEngine.Infrastructure;

namespace SunEngine.Controllers
{
    public abstract class BaseController : Controller 
    {
        protected readonly MyUserManager userManager;
        
        protected BaseController(MyUserManager userManager)
        {
            this.userManager = userManager;
        }
        
        public new MyClaimsPrincipal User => (MyClaimsPrincipal) base.User;

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