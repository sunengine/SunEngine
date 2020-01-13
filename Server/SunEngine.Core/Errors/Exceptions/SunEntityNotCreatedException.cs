namespace SunEngine.Core.Errors.Exceptions
{
	public class SunEntityNotCreatedException : SunDataBaseException
	{
		public SunEntityNotCreatedException(string entityName)
			: base($"{entityName} not created")
		{
		}
	}
}