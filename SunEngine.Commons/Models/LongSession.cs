using System;

namespace SunEngine.Commons.Models
{
    public class LongSession
    {
        public const int LongToken1Length = 16;
        public const int LongToken2Length = 16;
        
        public long Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public string LongToken1 { get; set; }
        public string LongToken2 { get; set; }
        public DateTime ExpirationDate { get; set; }
        public string DeviceInfo { get; set; }
    }
}