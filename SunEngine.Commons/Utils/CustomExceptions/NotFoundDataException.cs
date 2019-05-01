using System;

namespace SunEngine.Commons.Utils.CustomExceptions
{
    public class NotFoundDataException : Exception
    {
        public NotFoundDataException(string message) : base(message) { }
    }
}