using System;

namespace SunEngine.Core.Errors.Exceptions
{
	public class NotFoundServiceException : Exception
	{
		public NotFoundServiceException() : base()
		{
		}

		public NotFoundServiceException(string message)
			: base(message)
		{
		}
	}
}