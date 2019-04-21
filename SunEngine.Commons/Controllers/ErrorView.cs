using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Identity;

namespace SunEngine.Commons.Controllers
{
    public class ErrorView
    {
        public IList<ErrorObject> Errors { get; } = new List<ErrorObject>();

        public ErrorView()
        {
        }

        public ErrorView(string code, string description, bool isSoft = false, string message = null)
        {
            ErrorObject errorObject = new ErrorObject(code,description,isSoft,message);
            Errors.Add(errorObject);
        }

        public ErrorView(IEnumerable<IdentityError> identityErrors)
        {
            foreach (var identityError in identityErrors)
            {
                AddError(identityError.Code,identityError.Description);
            }
        }
        
        public static ErrorView Create(string code, string description, string message = null)
        {
            return new ErrorView(code, description, false, message);
        }

        public static ErrorView CreateSoft(string code, string description, string message = null)
        {
            return new ErrorView(code, description, true, message);
        }
        
        

        public void AddError(string code, string description, string message = null)
        {
            ErrorObject errorObject = new ErrorObject(code, description, false, message);
            Errors.Add(errorObject);
        }

        public void AddSoftError(string code, string description, string message = null)
        {
            ErrorObject errorObject = new ErrorObject(code, description, true, message);
            Errors.Add(errorObject);
        }
    }

    public class ErrorObject
    {
        public string Code { get; }

        public string Description { get; }

        public string Message { get; }

        public bool IsSoft { get; }

        public ErrorObject(string code, string description, bool isSoft = false, string message = null)
        {
            Code = code;
            Description = description;
            IsSoft = isSoft;
            Message = message;
        }

        public static ErrorObject Create(string code, string description, string message = null)
        {
            return new ErrorObject(code, description, false, message);
        }

        public static ErrorObject CreateSoft(string code, string description, string message = null)
        {
            return new ErrorObject(code, description, true, message);
        }
    }
}