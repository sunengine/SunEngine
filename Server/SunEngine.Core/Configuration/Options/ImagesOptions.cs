namespace SunEngine.Core.Configuration.Options
{
	public class ImagesOptions
	{
		public int MaxImageWidth { get; set; }
		public int MaxImageHeight { get; set; }
		public int ResizeMaxWidthPixels { get; set; }
		public int ResizeMaxHeightPixels { get; set; }
		public int PhotoMaxWidthPixels { get; set; }
		public int PhotoMaxHeightPixels { get; set; }
		public int AvatarSizePixels { get; set; }
		public bool AllowSvgUpload { get; set; }
		public bool AllowGifUpload { get; set; }
		public int ImageRequestSizeLimitBytes { get; set; }
	}
}