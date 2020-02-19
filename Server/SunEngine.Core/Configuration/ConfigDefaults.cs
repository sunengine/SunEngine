using System.Collections.Generic;
using SunEngine.Core.Configuration.Options;

namespace SunEngine.Core.Configuration
{
	public static class ConfigDefaults
	{
		public static readonly Dictionary<string, ConfigItem> ConfigurationItems = new Dictionary<string, ConfigItem>
		{
			["Global:SiteName"] = new StringItem("SunEngine", true),
			["Global:Locale"] = new EnumItem(Locale.Russian, true),
			["Global:SiteTitle"] = new HtmlStringItem("SunEngine Demo"),
			["Global:PageTitleTemplate"] = new StringItem("{pageTitle} - {siteName}"),
			["Global:HomePageRedirect"] = new StringItem(),
			["Global:DisallowRegistration"] = new BooleanItem(),
			["Global:ReadOnlyMode"] = new BooleanItem(),
			["Global:IconsSet"] = new EnumItem(IconsSet.LineAwesome),
			["Global:OpenExternalLinksAtNewTab"] = new BooleanItem(true),

			["Register:ConfirmText"] = new HtmlStringItem(),
			["Register:RequireUniqueEmail"] = new BooleanItem(true),
			["Register:AllowedUserNameCharacters"] =
				new StringItem("abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789 -"),

			["PasswordValidation:RequiredLength"] = new IntegerItem(6),
			["PasswordValidation:RequiredUniqueChars"] = new IntegerItem(2),
			["PasswordValidation:RequireDigit"] = new BooleanItem(),
			["PasswordValidation:RequireLowercase"] = new BooleanItem(),
			["PasswordValidation:RequireUppercase"] = new BooleanItem(),
			["PasswordValidation:RequireNonAlphanumeric"] = new BooleanItem(),

			["Dev:LogInitExtended"] = new BooleanItem(false, true),
			["Dev:LogRequests"] = new BooleanItem(false, true),
			["Dev:LogMoveTo"] = new BooleanItem(false, true),
			["Dev:ShowExceptions"] = new BooleanItem(false, true),
			["Dev:VueDevTools"] = new BooleanItem(false, true),
			["Dev:VueAppInWindow"] = new BooleanItem(false, true),

			["Cache:CurrentCachePolicy"] = new EnumItem(CachePolicy.AlwaysPolicy),
			["Cache:InvalidateCacheTime"] = new IntegerItem(15),

			["Images:MaxImageWidth"] = new IntegerItem(6000),
			["Images:MaxImageHeight"] = new IntegerItem(4000),
			["Images:ResizeMaxWidthPixels"] = new IntegerItem(1200),
			["Images:ResizeMaxHeightPixels"] = new IntegerItem(800),
			["Images:PhotoMaxWidthPixels"] = new IntegerItem(500),
			["Images:PhotoMaxHeightPixels"] = new IntegerItem(500),
			["Images:AvatarSizePixels"] = new IntegerItem(300),
			["Images:AllowGifUpload"] = new BooleanItem(true),
			["Images:AllowSvgUpload"] = new BooleanItem(true),
			["Images:ImageRequestSizeLimitBytes"] = new IntegerItem(10485760),

			#region Sanitizer

			["Sanitizer:SanitizeAdminMaterials"] = new BooleanItem(true),
			["Sanitizer:AllowedTags"] =
				new LongString(
					"a,b,strong,i,em,blockquote,ol,li,ul,ol,p,div,br,video,audio,source,span,img,code,pre,font,h3,h4,h5,h6"),
			["Sanitizer:AllowedAttributes"] =
				new LongString(
					"style,src,href,controls,autoplay,loop,alt,width,height,target,frameborder,allowfullscreen,download,controlsList,size"),
			["Sanitizer:AllowedClasses"] =
				new LongString(
					"float,margin,indent,padding,color,text-align,text-decoration,font-size,width,height,max-width"),
			["Sanitizer:AllowedCssProperties"] =
				new LongString(
					"float,margin,indent,padding,color,text-align,text-decoration,font-size,width,height,max-width"),
			["Sanitizer:AllowedImageDomains"] = new LongString(),
			["Sanitizer:AllowedVideoDomains"] = new LongString(
				"https://www.youtube.com/,http://www.youtube.com/,https://youtube.com/,http://youtube.com/,https://youtu.be/,http://youtu.be/,//youtube.com/,//youtu.be/,//www.youtube.com/,//www.youtu.be/,https://vk.com/,http://vk.com/,//vk.com/,https://player.vimeo.com,http://player.vimeo.com,//player.vimeo.com"),
			["Sanitizer:AllowedSchemes"] = new LongString(),

			#endregion

			["Email:Host"] = new StringItem("127.0.0.1"),
			["Email:Port"] = new IntegerItem(1025),
			["Email:Login"] = new StringItem("username"),
			["Email:Password"] = new StringItem("password"),
			["Email:EmailFromName"] = new StringItem("SunEngine Demo"),
			["Email:EmailFromAddress"] = new StringItem("SunEngine@demo.com"),
			["Email:UseSSL"] = new BooleanItem(true),

			#region EditorToolbars

			["Editor:MaterialToolbar"] = new JsonString(@"[
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
            ]"),

			["Editor:UserInformationToolbar"] = new JsonString(@"[
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
            ]"),
			["Editor:CommentToolbar"] = new JsonString(@"[
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
            ]"),
			["Editor:SendPrivateMessageToolbar"] = new JsonString(@"[
                ['bold', 'italic', 'strike', 'underline'],
                ['token', 'hr' ],
                ['quote', 'unordered', 'ordered' ],
                ['undo', 'redo','fullscreen']
            ]"),

			#endregion

			["Materials:SubTitleLength"] = new IntegerItem(80),
			["Materials:CommentsPageSize"] = new IntegerItem(5),
			["Materials:TimeToOwnEditInMinutes"] = new IntegerItem(15),
			["Materials:TimeToOwnDeleteInMinutes"] = new IntegerItem(15),
			["Materials:TimeToOwnMoveInMinutes"] = new IntegerItem(15),

			["Comments:TimeToOwnEditInMinutes"] = new IntegerItem(15),
			["Comments:TimeToOwnDeleteInMinutes"] = new IntegerItem(15),

			["Blog:PreviewLength"] = new IntegerItem(850),
			["Blog:PostsPageSize"] = new IntegerItem(8),

			["Articles:CategoryPageSize"] = new IntegerItem(12),

			["Forum:NewTopicsPageSize"] = new IntegerItem(15),
			["Forum:NewTopicsMaxPages"] = new IntegerItem(10),
			["Forum:ThreadMaterialsPageSize"] = new IntegerItem(12),

			["Captcha:CaptchaTimeoutSeconds"] = new IntegerItem(180),

			["Scheduler:LogJobs"] = new BooleanItem(),
			["Scheduler:SpamProtectionCacheClearMinutes"] = new IntegerItem(8),
			["Scheduler:JwtBlackListServiceClearMinutes"] = new IntegerItem(60),
			["Scheduler:LongSessionsClearDays"] = new IntegerItem(1),
			["Scheduler:ExpiredRegistrationUsersClearDays"] = new IntegerItem(1),
			["Scheduler:UploadVisitsToDataBaseMinutes"] = new IntegerItem(5),

			["Jwe:LongTokenLiveTimeDays"] = new IntegerItem(90),
			["Jwe:ShortTokenLiveTimeMinutes"] = new IntegerItem(20),
			["Jwe:Issuer"] = new StringItem("SunEngine Demo"),

			["Skins:CurrentSkinName"] = new StringItem("Default"),
			["Skins:PartialSkinsNames"] = new TokensItem("Branding"),
			["Skins:MaxArchiveSizeKb"] = new IntegerItem(20 * 1024),
			["Skins:MaxExtractArchiveSizeKb"] = new IntegerItem(60 * 1024)
		};
	}
}