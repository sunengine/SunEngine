using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace SunEngine.Core.Errors
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum ErrorType
    {
        System = 0,
        Soft = 1
    }
    
    public class Error
    {
        public string Code { get; set; }

        public string Description { get; set; }

        public string Message { get; set; }

        public string StackTrace  { get; set; }

        public ErrorType Type { get; set; }
        
        public int ResponseCode { get; set; }

        public Error()
        {
            
        }
        
        public Error(string code, string description, ErrorType type)
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