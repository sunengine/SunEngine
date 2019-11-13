using System.Collections.Generic;
using System.Linq;

namespace SunEngine.Core.Configuration
{
    public static class ConfigDefaults
    {
        public enum Test
        {
            TestValue1,
            TestValue2,
            TestValue3
        }
        
        public static readonly Dictionary<string, object> ConfigurationItems = new Dictionary<string, object>()
        {
            ["Test:Test1"] = Test.TestValue2,
            
            ["Sanitizer:AllowedTags"] = (LongString)"a,b,strong,i,em,blockquote,ol,li,ul,ol,p,div,br,video,audio,source,span,img,code,pre,font,h3,h4,h5,h6",
            ["Sanitizer:AllowedAttributes"] = (LongString)"style,src,href,controls,autoplay,loop,alt,width,height,target,frameborder,allowfullscreen,download,controlsList,size",
            ["Sanitizer:AllowedClasses"] = (LongString)"float,margin,indent,padding,color,text-align,text-decoration,font-size,width,height,max-width",
            ["Sanitizer:AllowedCssProperties"] = (LongString)"float,margin,indent,padding,color,text-align,text-decoration,font-size,width,height,max-width",
            ["Sanitizer:AllowedVideoDomains"] = (LongString)"https://www.youtube.com/,http://www.youtube.com/,https://youtube.com/,http://youtube.com/,https://youtu.be/,http://youtu.be/,//youtube.com/,//youtu.be/,//www.youtube.com/,//www.youtu.be/,https://vk.com/,http://vk.com/,//vk.com/,https://player.vimeo.com,http://player.vimeo.com,//player.vimeo.com",
            ["Sanitizer:AllowedImageDomains"] = (LongString)"",
            
            ["Global:SiteName"] = "SunEngine Demo",

            ["Dev:ShowExceptions"] = false,

            ["Images:MaxImageWidth"] = 3600,
            ["Images:MaxImageHeight"] = 2025,
            ["Images:ResizeMaxWidthPixels"] = 1200,
            ["Images:ResizeMaxHeightPixels"] = 800,
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
            
            ["FileLoading:MaxArchiveSize"] = 20 * 1024,
            ["FileLoading:MaxExtractArchiveSize"] = 60 * 1024,
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