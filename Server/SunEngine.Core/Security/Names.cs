using SunEngine.Core.Utils;

namespace SunEngine.Core.Security
{
	public static class RoleNames
	{
		public const string Admin = "Admin";
		public const string Registered = "Registered";
		public const string Unregistered = "Unregistered";
		public const string Banned = "Banned";

		public static readonly string AdminNormalized = Normalizer.Normalize(Admin);
		public static readonly string RegisteredNormalized = Normalizer.Normalize(Registered);
		public static readonly string UnregisteredNormalized = Normalizer.Normalize(Unregistered);
		public static readonly string BannedNormalized = Normalizer.Normalize(Banned);
	}

	public static class TokenClaimNames
	{
		public const string LongToken2CoockiName = "LAT2";

		/// <summary>
		/// Token stored in Cookies
		/// Needed to verify both long and short tokens
		/// </summary>
		public const string LongToken2Db = "LAT2";

		public const string LongToken2Ran = "LAT2R";

		public const string SessionId = "SID";
	}

	public static class Headers
	{
		public const string LongToken1HeaderName = "LongToken1";

		/// <summary>
		/// Header name to send tokens, LongToken1 and ShortAccessToken
		/// </summary>
		public const string TokensHeaderName = "Tokens";

		public const string TokensExpireValue = "expire";
	}

	public static class CipherSecrets
	{
		public const string ShortJwt = "ShortJwt";

		public const string Long2Jwt = "Long2Jwt";

		public const string EmailChange = "EmailChange";
	}
}