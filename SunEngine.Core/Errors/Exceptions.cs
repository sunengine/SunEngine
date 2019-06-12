using System;

namespace SunEngine.Core.Errors
{
    public class SunException : Exception
    {
        public SunException(string message = null, Exception innerException = null)
            : base(message, innerException)
        {
        }
    }

    public class SunDataBaseException : SunException
    {
        public SunDataBaseException(string message = null, Exception innerException = null)
            : base(message, innerException)
        {
        }
    }

    public class SunEntityNotCreatedException : SunDataBaseException
    {
        public SunEntityNotCreatedException(string entityName)
            : base($"{entityName} not created")
        {
        }
    }

    public class SunEntityNotUpdatedException : SunDataBaseException
    {
        public SunEntityNotUpdatedException(
            string entityName, object fieldValue, string searchByField = "Id")
            : base($"{entityName} not updated, searched by {searchByField}={fieldValue}")
        {
        }

        public SunEntityNotUpdatedException(
            string entityName, string message)
            : base($"{entityName} not updated, {message}")
        {
        }
    }

    public class SunEntityNotDeletedException : SunDataBaseException
    {
        public SunEntityNotDeletedException(
            string entityName, object fieldValue, string searchByField = "Id")
            : base($"{entityName} not deleted, searched by {searchByField}={fieldValue}")
        {
        }

        public SunEntityNotDeletedException(
            string entityName, string message)
            : base($"{entityName} not updated, {message}")
        {
        }
    }

    public class SunEntityNotFoundException : SunDataBaseException
    {
        public SunEntityNotFoundException(string entityName, object fieldValue, string searchByField = "Id")
            : base($"{entityName} not found, {searchByField}={fieldValue}")
        {
        }

        public SunEntityNotFoundException(string entityName, string message = null)
            : base($"{entityName} not found. Message: {message}")
        {
        }
    }

    public class SunParentEntityNotFoundException : SunDataBaseException
    {
        public SunParentEntityNotFoundException(
            string entityName, object fieldValue, string parentFieldName = "ParentId")
            : base($"{entityName} parent not found, {parentFieldName}={fieldValue}")
        {
        }
    }

    public class SunModelValidationException : SunException
    {
        public SunModelValidationException(string modelName, string propertyName, string message = "is null or empty")
            : base(
                $"{modelName}.{propertyName} {message}")
        {
        }
    }
}
