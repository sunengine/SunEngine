using System;

namespace SunEngine.Core.Exceptions.Database.Connection
{
    public class SunDatabaseException : Exception
    {
        public SunDatabaseException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}