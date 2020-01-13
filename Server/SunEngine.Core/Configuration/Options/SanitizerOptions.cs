using System.Linq;

namespace SunEngine.Core.Configuration.Options
{
	public class SanitizerOptions
	{
		public string AllowedTags { get; set; }
		public string AllowedAttributes { get; set; }
		public string AllowedClasses { get; set; }
		public string AllowedCssProperties { get; set; }
		public string AllowedVideoDomains { get; set; }
		public string AllowedImageDomains { get; set; }
		public string AllowedSchemes { get; set; }


		protected string[] _allowedTags;
		protected string[] _allowedAttributes;
		protected string[] _allowedClasses;
		protected string[] _allowedCssProperties;
		protected string[] _allowedVideoDomains;
		protected string[] _allowedImageDomains;
		protected string[] _allowedSchemes;


		public string[] AllowedTagsArr
		{
			get
			{
				if (_allowedTags == null)
					_allowedTags = AllowedTags.Split(",").Select(x => x.Trim()).ToArray();
				return _allowedTags;
			}
		}

		public string[] AllowedAttributesArr
		{
			get
			{
				if (_allowedAttributes == null)
					_allowedAttributes = AllowedAttributes.Split(",").Select(x => x.Trim()).ToArray();
				return _allowedAttributes;
			}
		}

		public string[] AllowedClassesArr
		{
			get
			{
				if (_allowedClasses == null)
					_allowedClasses = AllowedClasses.Split(",").Select(x => x.Trim()).ToArray();
				return _allowedClasses;
			}
		}

		public string[] AllowedCssPropertiesArr
		{
			get
			{
				if (_allowedCssProperties == null)
					_allowedCssProperties = AllowedCssProperties.Split(",").Select(x => x.Trim()).ToArray();
				return _allowedCssProperties;
			}
		}

		public string[] AllowedVideoDomainsArr
		{
			get
			{
				if (_allowedVideoDomains == null)
					_allowedVideoDomains = AllowedVideoDomains.Split(",").Select(x => x.Trim()).ToArray();
				return _allowedVideoDomains;
			}
		}

		public string[] AllowedImageDomainsArr
		{
			get
			{
				if (_allowedImageDomains == null)
					_allowedImageDomains = AllowedImageDomains.Split(",").Select(x => x.Trim()).ToArray();
				return _allowedImageDomains;
			}
		}

		public string[] AllowedSchemesArr
		{
			get
			{
				if (_allowedSchemes == null)
					_allowedSchemes = AllowedSchemes.Split(",").Select(x => x.Trim()).ToArray();
				return _allowedSchemes;
			}
		}
	}
}