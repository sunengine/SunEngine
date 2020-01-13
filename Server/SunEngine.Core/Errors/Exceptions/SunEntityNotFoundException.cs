namespace SunEngine.Core.Errors.Exceptions
{
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
}