using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace SunEngine.Commons.Errors
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum ErrorType
    {
        System = 0,
        Soft = 1
    }
    
    public class ErrorObject
    {
        public string Code { get; }

        public string Description { get; }

        public string Message { get; }

        public string StackTrace { get; }

        public ErrorType Type { get; }

        public ErrorObject(string code, string description, ErrorType type)
        {
            Code = code;
            Description = description;
            Type = type;
        }
        
        public ErrorObject(string code, string description, ErrorType type, string message)
        {
            Code = code;
            Description = description;
            Type = type;
            Message = message;
        }

        public ErrorObject(string code, string description, ErrorType type, Exception exception)
        {
            Code = code;
            Description = description;
            Type = type;
            Message = exception.Message;
            StackTrace = exception.StackTrace;
        }

        
    }
}