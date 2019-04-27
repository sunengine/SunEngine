using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace SunEngine.Commons.Misc
{
    public class ErrorView
    {
        public IList<ErrorObject> Errors { get; } = new List<ErrorObject>();

        public ErrorView()
        {
        }

        public ErrorView(string code, string description, ErrorType type)
        {
            ErrorObject errorObject = new ErrorObject(code, description, type);
            Errors.Add(errorObject);
        }

        public ErrorView(string code, string description, ErrorType type, string message)
        {
            ErrorObject errorObject = new ErrorObject(code, description, type, message);
            Errors.Add(errorObject);
        }

        public ErrorView(string code, string description, ErrorType type, Exception exception)
        {
            ErrorObject errorObject = new ErrorObject(code, description, type, exception);
            Errors.Add(errorObject);
        }

        public ErrorView(IEnumerable<IdentityError> identityErrors)
        {
            foreach (var identityError in identityErrors)
            {
                AddError(identityError.Code, identityError.Description, ErrorType.System);
            }
        }


        public static ErrorView SoftError(string code, string description)
        {
            return new ErrorView(code, description, ErrorType.Soft);
        }

        public static ErrorView SoftError(string code, string description, string message)
        {
            return new ErrorView(code, description, ErrorType.Soft, message);
        }

        public static ErrorView SoftError(string code, string description, Exception exception)
        {
            return new ErrorView(code, description, ErrorType.Soft, exception);
        }

        public static ErrorView Unauthorized()
        {
            return new ErrorView("Unauthorized", "Not unauthorized for this request", ErrorType.System);
        }

        public static ErrorView BadRequest()
        {
            return new ErrorView("BadRequest", "BadRequest happened", ErrorType.System);
        }

        public static ErrorView ServerError(Exception exception = null)
        {
            return new ErrorView("ServerError", "Server error. Something goes wrong", ErrorType.System, exception);
        }

        public void AddError(string code, string description, ErrorType type, string message = null)
        {
            ErrorObject errorObject = new ErrorObject(code, description, type, message);
            Errors.Add(errorObject);
        }

        public void AddError(string code, string description, ErrorType type, Exception exception)
        {
            ErrorObject errorObject = new ErrorObject(code, description, type, exception);
            Errors.Add(errorObject);
        }
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum ErrorType
    {
        System = 0,
        Soft = 1
    }

    public class ErrorObject
    {
        public string Code { get; }

        public string Description { get; }

        public string Message { get; }

        public string StackTrace { get; }

        public ErrorType Type { get; }

        public ErrorObject(string code, string description, ErrorType type)
        {
            Code = code;
            Description = description;
            Type = type;
        }

        public ErrorObject(string code, string description, ErrorType type, Exception exception)
        {
            Code = code;
            Description = description;
            Type = type;
            Message = exception?.Message;
            StackTrace = exception?.StackTrace;
        }

        public ErrorObject(string code, string description, ErrorType type, string message)
        {
            Code = code;
            Description = description;
            Type = type;
            Message = message;
        }
    }
}