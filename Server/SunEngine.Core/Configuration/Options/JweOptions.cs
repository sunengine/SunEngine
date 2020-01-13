namespace SunEngine.Core.Configuration.Options
{
	public class JweOptions
	{
		public string Issuer { get; set; }
		public int LongTokenLiveTimeDays { get; set; }
		public int ShortTokenLiveTimeMinutes { get; set; }
	}
}