namespace SunEngine.Core.Configuration.Options
{
	public enum CachePolicy
	{
		AlwaysPolicy,
		NeverPolicy,
		CustomPolicy
	}

	public class CacheOptions
	{
		public CachePolicy CurrentCachePolicy { get; set; }
		public int InvalidateCacheTime { get; set; }
	}
}