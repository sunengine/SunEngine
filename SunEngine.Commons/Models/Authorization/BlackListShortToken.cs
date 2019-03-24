using System;

namespace SunEngine.Commons.Models.Authorization
{
    public class BlackListShortToken
    {
        public string TokenId { get; set; }
        public DateTime Expire { get; set; }
    }
}