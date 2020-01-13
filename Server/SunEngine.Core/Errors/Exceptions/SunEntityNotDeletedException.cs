namespace SunEngine.Core.Errors.Exceptions
{
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
}