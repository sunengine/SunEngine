using System;

namespace SunEngine.Core.Exceptions.Database.Connection
{
    public class SunDatabaseConnectionException : Exception 
    {
        public SunDatabaseConnectionException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}