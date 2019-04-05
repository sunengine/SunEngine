using System;

namespace SunEngine.Commons.Utils.CustomExceptions
{
    public class NotFoundServiceException : Exception
    {
        public NotFoundServiceException(string message)
            : base(message)
        {
            
        }
    }
}