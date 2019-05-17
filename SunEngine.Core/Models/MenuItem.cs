namespace SunEngine.Core.Models
{
    public class MenuItem
    {
        public int Id { get; set; }
        public int? ParentId { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public string SubTitle { get; set; }
        public string RouteName { get; set; }
        public string RouteParamsJson { get; set; }
        public string SettingsJson { get; set; }
        public string CssClass { get; set; }
        public string ExternalUrl { get; set; }
        public bool IsSeparator { get; set; }
        public string SortNumber { get; set; }
        public string Icon { get; set; }
        public string CustomIcon { get; set; }
    }
}