using SunEngine.Controllers;
using SunEngine.Models;

namespace SunEngine.Services
{
    public class ServiceResult
    {
        public bool Succeeded;
        public ErrorViewModel Error;

        public bool Failed => !Succeeded;

        public static ServiceResult BadResult(ErrorViewModel error)
        {
            return new ServiceResult
            {
                Succeeded = false,
                Error = error
            };
        }
        
        public static ServiceResult OkResult()
        {
            return new ServiceResult
            {
                Succeeded = true
            };
        }
    }
    
    public class UserServiceResult : ServiceResult
    {
        public User user { get; set; }

        public static UserServiceResult OkResult(User user)
        {
            return new UserServiceResult
            {
                Succeeded = true,
                user = user
            };
        }
        
        public static UserServiceResult BadResult(ErrorViewModel error)
        {
            return new UserServiceResult
            {
                Succeeded = false,
                Error = error
            };
        }
    }
}