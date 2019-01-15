namespace SunEngine.Options
{
    public class ImagesOptions
    {
        public string UploadDir { get; set; }
        public bool DoResize { get; set; }
        public int MaxWidthPixels { get; set; }
        public int MaxHeightPixels { get; set; }
        public int PhotoMaxWidthPixels { get; set; }
        public int PhotoMaxHeightPixels { get; set; }
        public int AvatarSizePixels { get; set; }
    }
}