namespace SunEngine.Core.Configuration.Options
{
    public class SchedulerOptions
    {
        public int SpamProtectionCacheClearMinutes { get; set; }
        public int JwtBlackListServiceClearMinutes { get; set; }
        public int LongSessionsClearDays { get; set; }
        public int ExpiredRegistrationUsersClearDays { get; set; }
        public int UploadVisitsToDataBaseMinutes { get; set; }
    }
}
