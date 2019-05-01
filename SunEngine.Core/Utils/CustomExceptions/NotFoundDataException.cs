using System;

namespace SunEngine.Core.Utils.CustomExceptions
{
    public class NotFoundDataException : Exception
    {
        public NotFoundDataException(string message) : base(message) { }
    }
}