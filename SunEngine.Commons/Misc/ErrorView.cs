using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace SunEngine.Commons.Misc
{
    public class ErrorView
    {
        public IList<ErrorObject> Errors { get; } = new List<ErrorObject>();

        public ErrorView()
        {
        }

        public ErrorView(string code, string description)
        {
            ErrorObject errorObject = new ErrorObject(code, description);
            Errors.Add(errorObject);
        }

        public ErrorView(string code, string description, string message)
        {
            ErrorObject errorObject = new ErrorObject(code, description, false, message);
            Errors.Add(errorObject);
        }

        public ErrorView(string code, string description, Exception exception)
        {
            ErrorObject errorObject = new ErrorObject(code, description, false, exception);
            Errors.Add(errorObject);
        }

        public ErrorView(string code, string description, bool isSoft, string message = null)
        {
            ErrorObject errorObject = new ErrorObject(code, description, isSoft, message);
            Errors.Add(errorObject);
        }

        public ErrorView(string code, string description, bool isSoft, Exception exception)
        {
            ErrorObject errorObject = new ErrorObject(code, description, isSoft, exception);
            Errors.Add(errorObject);
        }

        public ErrorView(IEnumerable<IdentityError> identityErrors)
        {
            foreach (var identityError in identityErrors)
            {
                AddError(identityError.Code, identityError.Description);
            }
        }

        public static ErrorView SoftError(string code, string description)
        {
            return new ErrorView(code, description, true);
        }

        public static ErrorView SoftError(string code, string description, string message)
        {
            return new ErrorView(code, description, true, message);
        }

        public static ErrorView SoftError(string code, string description, Exception exception)
        {
            return new ErrorView(code, description, true, exception);
        }

        public static ErrorView Unauthorized()
        {
            return new ErrorView("Unauthorized", "Not unauthorized for this request");
        }

        public static ErrorView BadRequest()
        {
            return new ErrorView("BadRequest", "BadRequest happened");
        }

        public static ErrorView ServerError(Exception exception = null)
        {
            return new ErrorView("ServerError", "Server error. Something goes wrong", exception);
        }

        public void AddError(string code, string description, string message = null)
        {
            ErrorObject errorObject = new ErrorObject(code, description, false, message);
            Errors.Add(errorObject);
        }

        public void AddError(string code, string description, Exception exception)
        {
            ErrorObject errorObject = new ErrorObject(code, description, false, exception);
            Errors.Add(errorObject);
        }

        public void AddSoftError(string code, string description, string message = null)
        {
            ErrorObject errorObject = new ErrorObject(code, description, true, message);
            Errors.Add(errorObject);
        }

        public void AddSoftError(string code, string description, Exception exception)
        {
            ErrorObject errorObject = new ErrorObject(code, description, true, exception);
            Errors.Add(errorObject);
        }
    }

    public class ErrorObject
    {
        public string Code { get; }

        public string Description { get; }

        public string Message { get; }

        public string StackTrace { get; }

        public bool IsSoft { get; }

        public ErrorObject(string code, string description)
        {
            Code = code;
            Description = description;
            IsSoft = false;
        }

        public ErrorObject(string code, string description, bool isSoft)
        {
            Code = code;
            Description = description;
            IsSoft = isSoft;
        }

        public ErrorObject(string code, string description, bool isSoft, Exception exception)
        {
            Code = code;
            Description = description;
            IsSoft = isSoft;
            Message = exception?.Message;
            StackTrace = exception?.StackTrace;
        }

        public ErrorObject(string code, string description, bool isSoft, string message)
        {
            Code = code;
            Description = description;
            IsSoft = isSoft;
            Message = message;
        }
    }
}