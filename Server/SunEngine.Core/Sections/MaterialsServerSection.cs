using SunEngine.Core.Configuration.ConfigItemType;
using SunEngine.Core.Models;

namespace SunEngine.Core.Sections
{
	public class MaterialsServerSection : ServerSection
	{
		[ConfigItem(typeof(CategoriesItem))] public string CategoriesNames { get; set; }
		[ConfigItem(typeof(IntegerItem))] public int PageSize { get; set; }= 20;
		
	}
}
