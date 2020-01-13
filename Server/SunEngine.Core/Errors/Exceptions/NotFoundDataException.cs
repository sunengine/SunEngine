using System;

namespace SunEngine.Core.Errors.Exceptions
{
	public class NotFoundDataException : Exception
	{
		public NotFoundDataException(string message) : base(message)
		{
		}
	}
}