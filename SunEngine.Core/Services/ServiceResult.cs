using SunEngine.Core.Errors;
using SunEngine.Core.Models;

namespace SunEngine.Core.Services
{
    /// <summary>
    /// Class for Ok or Error result for async methods
    /// </summary>
    public class ServiceResult
    {
        public bool Succeeded;
        public ErrorView Error;

        public bool Failed => !Succeeded;

        public static ServiceResult BadResult(ErrorView error = null)
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
        
        public static UserServiceResult BadResult(ErrorView error = null)
        {
            return new UserServiceResult
            {
                Succeeded = false,
                Error = error
            };
        }
    }
}