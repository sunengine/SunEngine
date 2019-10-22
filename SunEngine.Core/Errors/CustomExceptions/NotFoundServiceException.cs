using System;

namespace SunEngine.Core.Utils.CustomExceptions
{
    public class NotFoundServiceException : Exception
    {
        public NotFoundServiceException() : base() { }
        
        public NotFoundServiceException(string message)
            : base(message)
        {
            
        }
    }
}