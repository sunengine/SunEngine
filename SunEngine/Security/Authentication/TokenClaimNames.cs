namespace SunEngine.Security
{
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
        public const string TokensHeaderName = "TOKENS";

        public const string TokensExpireHeaderName = "TOKENSEXPIRE";
        
        public const string TokensExpireHeaderValue = "TRUE";

    }
}