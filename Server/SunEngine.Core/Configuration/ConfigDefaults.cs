using System.Collections.Generic;
using SunEngine.Core.Configuration.Options;

namespace SunEngine.Core.Configuration
{
	public static class ConfigDefaults
	{
		public enum Locale
		{
			Russian,
			English,
		}
		
		public enum IconsSet
		{
			LineAwesome,
			FontAwesome,
		}

		public static readonly Dictionary<string, object> ConfigurationItems = new Dictionary<string, object>()
		{
			["Global:SiteName"] = "SunEngine",
			["Global:Locale"] = Locale.Russian,
			["Global:SiteTitle"] = "SunEngine Demo",
			["Global:SiteSubTitle"] = "",
			["Global:PageTitleTemplate"] = "{pageTitle} - {siteName}",
			["Global:IconsSet"] = IconsSet.LineAwesome,
			["Global:OpenExternalLinksAtNewTab"] = true,
			
			["Dev:LogInitExtended"] = false,
			["Dev:LogRequests"] = false,
			["Dev:LogMoveTo"] = false,
			["Dev:ShowExceptions"] = false,
			["Dev:VueDevTools"] = false,
			["Dev:VueAppInWindow"] = false,

			["Cache:CurrentCachePolicy"] = CachePolicy.AlwaysPolicy,
			["Cache:InvalidateCacheTime"] = 15,

			["Images:MaxImageWidth"] = 6000,
			["Images:MaxImageHeight"] = 4000,
			["Images:ResizeMaxWidthPixels"] = 1200,
			["Images:ResizeMaxHeightPixels"] = 800,
			["Images:PhotoMaxWidthPixels"] = 500,
			["Images:PhotoMaxHeightPixels"] = 500,
			["Images:AvatarSizePixels"] = 300,
			["Images:AllowGifUpload"] = true,
			["Images:AllowSvgUpload"] = true,
			["Images:ImageRequestSizeLimitBytes"] = 10485760,

			["Sanitizer:AllowedTags"] =
				(LongString)
				"a,b,strong,i,em,blockquote,ol,li,ul,ol,p,div,br,video,audio,source,span,img,code,pre,font,h3,h4,h5,h6",
			["Sanitizer:AllowedAttributes"] =
				(LongString)
				"style,src,href,controls,autoplay,loop,alt,width,height,target,frameborder,allowfullscreen,download,controlsList,size",
			["Sanitizer:AllowedClasses"] =
				(LongString)
				"float,margin,indent,padding,color,text-align,text-decoration,font-size,width,height,max-width",
			["Sanitizer:AllowedCssProperties"] =
				(LongString)
				"float,margin,indent,padding,color,text-align,text-decoration,font-size,width,height,max-width",
			["Sanitizer:AllowedImageDomains"] = (LongString) "",
			["Sanitizer:AllowedVideoDomains"] =
				(LongString)
				"https://www.youtube.com/,http://www.youtube.com/,https://youtube.com/,http://youtube.com/,https://youtu.be/,http://youtu.be/,//youtube.com/,//youtu.be/,//www.youtube.com/,//www.youtu.be/,https://vk.com/,http://vk.com/,//vk.com/,https://player.vimeo.com,http://player.vimeo.com,//player.vimeo.com",
			["Sanitizer:AllowedSchemes"] = (LongString) "",

			["Email:Host"] = "127.0.0.1",
			["Email:Port"] = 1025,
			["Email:Login"] = "username",
			["Email:Password"] = "password",
			["Email:EmailFromName"] = "SunEngine Demo",
			["Email:EmailFromAddress"] = "SunEngine@demo.com",
			["Email:UseSSL"] = true,

			#region EditorToolbars

			["Editor:MaterialToolbar"] = (JsonString) @"[
	          ['bold', 'italic', 'strike', 'underline'],
              ['token', 'link', 'addImages'],
              ['hr'],
              [{
              'icon': 'fas fa-heading',
              'fixedLabel': true,
              'fixedIcon': true,
              'list': 'no-icons',
              'options': ['p', 'h3', 'h4', 'h5', 'h6', 'code']
            },
            {
              'icon': 'fas fa-text-height',
              'fixedLabel': true,
              'fixedIcon': true,
              'list': 'no-icons',
              'options': ['size-1', 'size-2', 'size-3', 'size-4', 'size-5', 'size-6', 'size-7']
            },
            'quote',
            'removeFormat'
            ],
            ['unordered', 'ordered', 'outdent', 'indent',
            {
              'icon': 'fas fa-align-center',
              'fixedLabel': true,
              'options': ['left', 'center', 'right', 'justify']
            }
            ],
            ['undo', 'redo'],
            ['viewsource', 'fullscreen']
            ]",

			["Editor:UserInformationToolbar"] = (JsonString) @"[
              ['bold', 'italic', 'strike', 'underline', 'subscript', 'superscript'],
              ['token', 'hr', 'link', 'addImages'],
              [
                {
                  'icon': 'fas fa-heading',
                  'fixedLabel': true,
                  'list': 'no-icons',
                  'options': ['p', 'h2', 'h3', 'h4', 'h5', 'h6', 'code']
                },
                {
                  'icon':  'fas fa-text-height',
                  'fixedLabel': true,
                  'fixedIcon': true,
                  'list': 'no-icons',
                  'options': ['size-1', 'size-2', 'size-3', 'size-4', 'size-5', 'size-6', 'size-7']
                },
                'quote',
                'removeFormat'
              ],
              ['unordered', 'ordered', 'outdent', 'indent',
                {
                  'icon': 'fas fa-align-center',
                  'fixedLabel': true,
                  'options': ['left', 'center', 'right', 'justify']
                }
              ],
              ['undo', 'redo'],
              [ 'viewsource', 'fullscreen']
            ]",
			["Editor:CommentToolbar"] = (JsonString) @"[
              ['bold', 'italic', 'strike', 'underline'],
              ['token', 'hr', 'link', 'addImages'],
              [
                {
                  'icon': 'fas fa-heading',
                  'fixedLabel': true,
                  'list': 'no-icons',
                  'options': ['p', 'h2', 'h3', 'h4', 'h5', 'h6', 'code']
                },
                {
                  'icon': 'fas fa-text-height',
                  'fixedLabel': true,
                  'fixedIcon': true,
                  'list': 'no-icons',
                  'options': ['size-1', 'size-2', 'size-3', 'size-4', 'size-5', 'size-6', 'size-7']
                },
                'quote',
                'removeFormat'
              ],
              ['unordered', 'ordered'],
              ['undo', 'redo'],
              ['viewsource', 'fullscreen']
            ]",
			["Editor:SendPrivateMessageToolbar"] = (JsonString) @"[
                ['bold', 'italic', 'strike', 'underline'],
                ['token', 'hr' ],
                ['quote', 'unordered', 'ordered' ],
                ['undo', 'redo','fullscreen']
            ]",

			#endregion

			["Materials:SubTitleLength"] = 80,
			["Materials:CommentsPageSize"] = 5,
			["Materials:TimeToOwnEditInMinutes"] = 15,
			["Materials:TimeToOwnDeleteInMinutes"] = 15,
			["Materials:TimeToOwnMoveInMinutes"] = 15,

			["Comments:TimeToOwnEditInMinutes"] = 15,
			["Comments:TimeToOwnDeleteInMinutes"] = 15,

			["Blog:PreviewLength"] = 850,
			["Blog:PostsPageSize"] = 8,

			["Articles:CategoryPageSize"] = 12,

			["Forum:NewTopicsPageSize"] = 15,
			["Forum:NewTopicsMaxPages"] = 10,
			["Forum:ThreadMaterialsPageSize"] = 12,

			["Captcha:CaptchaTimeoutSeconds"] = 180,

			["Scheduler:LogJobs"] = false,
			["Scheduler:SpamProtectionCacheClearMinutes"] = 8,
			["Scheduler:JwtBlackListServiceClearMinutes"] = 60,
			["Scheduler:LongSessionsClearDays"] = 1,
			["Scheduler:ExpiredRegistrationUsersClearDays"] = 1,
			["Scheduler:UploadVisitsToDataBaseMinutes"] = 5,

			["Jwe:LongTokenLiveTimeDays"] = 90,
			["Jwe:ShortTokenLiveTimeMinutes"] = 20,
			["Jwe:Issuer"] = "SunEngine Demo",

			["Skins:CurrentSkinName"] = "Default",
			["Skins:PartialSkinsNames"] = "Branding",
			["Skins:MaxArchiveSizeKb"] = 20 * 1024,
			["Skins:MaxExtractArchiveSizeKb"] = 60 * 1024
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

	public class JsonString
	{
		public string Value { get; set; }

		public static implicit operator JsonString(string str)
		{
			return new JsonString(str.Replace("'", "\""));
		}

		public static explicit operator string(JsonString str)
		{
			return str.ToString();
		}

		public JsonString(string value)
		{
			Value = value.Replace("'", "\"");
		}

		public override string ToString()
		{
			return Value;
		}
	}
}