namespace SunEngine.Core.Configuration.Options
{
    public class ImagesOptions
    {
        public string ImagesUploadDir { get; set; }
        
        public string ImagesUploadUrl { get; set; }

        public bool DoResize { get; set; }
        public int MaxWidthPixels { get; set; }
        public int MaxHeightPixels { get; set; }
        public int PhotoMaxWidthPixels { get; set; }
        public int PhotoMaxHeightPixels { get; set; }
        public int AvatarSizePixels { get; set; }
        public bool AllowSvgUpload { get; set; }
        public bool AllowGifUpload { get; set; }
        public int ImageRequestSizeLimitBytes { get; set; }
    }
}