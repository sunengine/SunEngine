using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace SunEngine.Core.Errors
{
    public class ErrorList
    {
        public IList<Error> Errors { get; } = new List<Error>();

        public ErrorList()
        {
        }

        public ErrorList(Error error)
        {
            Errors.Add(error);
        }
        
        public ErrorList(string code, string description, ErrorType type)
        {
            Error error = new Error(code, description, type);
            Errors.Add(error);
        }

        public ErrorList(string code, string description, ErrorType type, string message)
        {
            Error error = new Error(code, description, type, message);
            Errors.Add(error);
        }

        public ErrorList(string code, string description, ErrorType type, Exception exception)
        {
            Error error = new Error(code, description, type, exception);
            Errors.Add(error);
        }

        public ErrorList(IEnumerable<IdentityError> identityErrors)
        {
            foreach (var identityError in identityErrors)
            {
                AddError(
                    new Error
                    {
                        Code = identityError.Code,
                        Description = identityError.Description,
                        Type = ErrorType.System
                    });
            }
        }

        public void AddError(Error error)
        {
            this.Errors.Add(error);
        }


        public static ErrorList SoftError(string code, string description)
        {
            return new ErrorList(code, description, ErrorType.Soft);
        }

      
        
    }
}