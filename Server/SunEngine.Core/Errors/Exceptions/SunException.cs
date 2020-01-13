using System;

namespace SunEngine.Core.Errors.Exceptions
{
	public class SunException : Exception
	{
		public SunException(string message = null, Exception innerException = null)
			: base(message, innerException)
		{
		}
	}
}