using System;
using System.Text.RegularExpressions;

namespace SunEngine.Commons.Configuration.Options
{
    public class EmailSenderOptions
    {
        public string EmailFromName { get; set; }
        public string EmailFromAddress { get; set; }
        public bool UseSSL { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string Host { get; set; }
        public int Port { get; set; }
    }
}