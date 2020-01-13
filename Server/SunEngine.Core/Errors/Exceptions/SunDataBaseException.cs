using System;

namespace SunEngine.Core.Errors.Exceptions
{
	public class SunDataBaseException : SunException
	{
		public SunDataBaseException(string message = null, Exception innerException = null)
			: base(message, innerException)
		{
		}
	}
}