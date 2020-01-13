using System;
using System.Text.Json.Serialization;

namespace SunEngine.Core.Errors
{
	[JsonConverter(typeof(JsonStringEnumConverter))]
	public enum ErrorType
	{
		System = 0,
		Soft = 1
	}

	public class Error
	{
		public string Code { get; set; }

		public string Description { get; set; }

		private string _message;

		public string Message
		{
			get => ErrorsMode.ShowErrors ? _message : null;
			set => _message = value;
		}

		private string _stackTrace;

		public string StackTrace
		{
			get => ErrorsMode.ShowErrors ? _stackTrace : null;
			set => _stackTrace = value;
		}

		public ErrorType Type { get; set; }

		public int ResponseCode { get; set; }

		public Error()
		{
		}

		public Error(string code, string description, ErrorType type = ErrorType.System)
		{
			Code = code;
			Description = description;
			Type = type;
		}

		public Error(string code, string description, ErrorType type, string message)
		{
			Code = code;
			Description = description;
			Type = type;
			Message = message;
		}

		public Error(string code, string description, ErrorType type, Exception exception)
		{
			Code = code;
			Description = description;
			Type = type;
			Message = exception.Message;
			StackTrace = exception.StackTrace;
		}
	}
}