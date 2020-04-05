using SunEngine.Core.Cache.CacheModels;

namespace SunEngine.Core.Cache.CachePolicy
{
  public class BaseCachePolicy : ICachePolicy
  {
    public bool CanCache(CategoryCached category, RequestOptions options)
    {
      return options.PageNumber.HasValue && options.PageNumber == 1;
    }

    public bool CanCache(SectionServerCached component, RequestOptions options)
    {
      return options.PageNumber.HasValue && options.PageNumber == 1;
    }
  }
}
