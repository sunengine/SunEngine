using SunEngine.Core.Configuration.ConfigItemType;

namespace SunEngine.Core.SectionsData
{
	public class MaterialsServerSectionData : CategoryServerSectionData
	{
    [ConfigItem(typeof(IntegerItem))] public int PageSize { get; set; } = 5; // Поменяй пожалуйста это значение, я не знаю какое ставить, это для примера
	}
}
