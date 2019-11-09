using System.Collections.Generic;
using System.Linq;

namespace SunEngine.Core.Configuration
{
    public static class ConfigDefaults
    {
        public enum BBB1
        {
            Hi1,
            Hi2,
            Hi3
        }
        
        public static readonly Dictionary<string, object> ConfigurationItems = new Dictionary<string, object>()
        {
            ["AAA:Test1"] = (LongString) "Test data 1",
            ["AAA:Test2"] = (LongString) string.Join(" ", Enumerable.Repeat("test data", 30)),
            
            ["BBB:Enum1"] = BBB1.Hi2,
            ["BBB:Enum2"] = BBB1.Hi3,

            ["Global:SiteName"] = "SunEngine Demo",

            ["Dev:ShowExceptions"] = false,

            ["Images:MaxWidthPixels"] = 1200,
            ["Images:MaxHeightPixels"] = 800,
            ["Images:PhotoMaxWidthPixels"] = 500,
            ["Images:PhotoMaxHeightPixels"] = 500,
            ["Images:AvatarSizePixels"] = 300,
            ["Images:AllowGifUpload"] = true,
            ["Images:AllowSvgUpload"] = true,
            ["Images:ImageRequestSizeLimitBytes"] = 10485760,
            ["Images:DoResize"] = true,

            ["Comments:TimeToOwnEditInMinutes"] = 15,
            ["Comments:TimeToOwnDeleteInMinutes"] = 15,

            ["Materials:CommentsPageSize"] = 5,
            ["Materials:TimeToOwnEditInMinutes"] = 15,
            ["Materials:TimeToOwnDeleteInMinutes"] = 15,
            ["Materials:TimeToOwnMoveInMinutes"] = 15,
            ["Materials:SubTitleLength"] = 80,

            ["Forum:ThreadMaterialsPageSize"] = 5,
            ["Forum:NewTopicsPageSize"] = 5,
            ["Forum:NewTopicsMaxPages"] = 8,

            ["Articles:CategoryPageSize"] = 8,

            ["Blog:PostsPageSize"] = 8,
            ["Blog:PreviewLength"] = 850,

            ["Jwe:LongTokenLiveTimeDays"] = 90,
            ["Jwe:ShortTokenLiveTimeMinutes"] = 20,
            ["Jwe:Issuer"] = "SunEngine Demo",

            ["Scheduler:SpamProtectionCacheClearMinutes"] = 8,
            ["Scheduler:JwtBlackListServiceClearMinutes"] = 60,
            ["Scheduler:LongSessionsClearDays"] = 1,
            ["Scheduler:ExpiredRegistrationUsersClearDays"] = 1,
            ["Scheduler:UploadVisitsToDataBaseMinutes"] = 5,

            ["Email:Host"] = "127.0.0.1",
            ["Email:Port"] = 1025,
            ["Email:Login"] = "username",
            ["Email:Password"] = "password",
            ["Email:EmailFromName"] = "SunEngine Demo",
            ["Email:EmailFromAddress"] = "SunEngine@demo.com",
        };
    }

    public class LongString
    {
        public string Value { get; set; }

        public static implicit operator LongString(string str)
        {
            return new LongString(str);
        }

        public static explicit operator string(LongString str)
        {
            return str.ToString();
        }

        public LongString(string value)
        {
            Value = value;
        }

        public override string ToString()
        {
            return Value;
        }
    }
}