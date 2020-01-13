using System;

namespace SunEngine.Core.Errors
{
	public static class Errors
	{
		public static Error Unauthorized()
		{
			return new Error("Unauthorized", "Not unauthorized for this request", ErrorType.System);
		}

		public static Error BadRequest()
		{
			return new Error("BadRequest", "BadRequest happened", ErrorType.System);
		}

		public static Error ServerError(Exception exception = null)
		{
			return new Error("ServerError", "Server error. Something goes wrong", ErrorType.System, exception);
		}

		public static Error ValidationError()
		{
			return new Error("ValidationError", "Data validation failed", ErrorType.Soft);
		}
	}
}