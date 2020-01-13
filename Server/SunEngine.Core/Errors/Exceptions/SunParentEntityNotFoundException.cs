namespace SunEngine.Core.Errors.Exceptions
{
	public class SunParentEntityNotFoundException : SunDataBaseException
	{
		public SunParentEntityNotFoundException(
			string entityName, object fieldValue, string parentFieldName = "ParentId")
			: base($"{entityName} parent not found, {parentFieldName}={fieldValue}")
		{
		}
	}
}