namespace SunEngine.Core.Errors.Exceptions
{
	public class SunErrorException : SunException
	{
		public Error Error { get; set; }

		public SunErrorException(Error error)
		{
			Error = error;
		}
	}
}