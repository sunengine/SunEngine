namespace SunEngine.Core.Errors.Exceptions
{
	public class SunModelValidationException : SunException
	{
		public SunModelValidationException(string modelName, string propertyName, string message = "is null or empty")
			: base(
				$"{modelName}.{propertyName} {message}")
		{
		}
	}
}