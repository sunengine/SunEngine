using System;

namespace SunEngine.Core.Errors
{
    public class ParentCategoryNotFoundByIdException : Exception
    {
        public ParentCategoryNotFoundByIdException(int? parentId) : base($"Parent category (id:{parentId}) not found. Can not add category without parent")
        {
        }
    }

    public class InvalidModelException : Exception
    {
        public InvalidModelException(string modelName, string propertyName, string message = "is null or empty") : base($"{modelName}.{propertyName} {message}")
        {
        }
    }
}