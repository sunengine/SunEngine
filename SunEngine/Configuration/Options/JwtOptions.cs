namespace SunEngine.Configuration.Options
{
    public class JwtOptions
    {
        public string ShortJwtSecurityKey { get; set; }
        public string LongJwtSecurityKey { get; set; }
        public string Issuer { get; set; }
        public string SecurityKeyEmailChange { get; set; }
    }
}