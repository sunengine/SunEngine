using System;
using System.Text.RegularExpressions;

namespace SunEngine.Configuration.Options
{
    public class EmailSenderOptions
    {
        private string _smtpConfig;
        public string SmtpConfig
        {
            get => _smtpConfig;
            set
            {
                _smtpConfig = value;

                // SmtpConfig is in username:password@localhost:1025 format; extract the part
                var smtpConfigPartsRegEx = new Regex(@"(.*)\:(.*)@(.+)\:(.+)");
                var smtpConfigPartsMatch = smtpConfigPartsRegEx.Match(value);

                Username = smtpConfigPartsMatch.Groups[1].Value;
                Password = smtpConfigPartsMatch.Groups[2].Value;
                Host = smtpConfigPartsMatch.Groups[3].Value;
                Port = Convert.ToInt32(smtpConfigPartsMatch.Groups[4].Value);
            }
        }

        public string EmailFromName { get; set; }
        public string EmailFromAddress { get; set; }
        public bool UseSSL { get; set; }

        public string Username { get; protected set; }
        public string Password { get; protected set; }
        public string Host { get; protected set; }
        public int Port { get; protected set; }
    }
}