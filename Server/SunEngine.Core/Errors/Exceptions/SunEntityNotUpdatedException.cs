namespace SunEngine.Core.Errors.Exceptions
{
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
}