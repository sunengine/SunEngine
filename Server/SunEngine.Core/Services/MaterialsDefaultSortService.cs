using System;
using System.Collections.Generic;
using System.Linq;
using SunEngine.Core.Models.Materials;
using SunEngine.Core.Presenters;

namespace SunEngine.Core.Services
{
  public static class MaterialsDefaultSortService
  {
    //переделать под модели а не презентеры.
    public static Dictionary<string, Func<IQueryable<Material>, IOrderedQueryable<Material>>> DefaultSortOptions => 
      new Dictionary<string, Func<IQueryable<Material>, IOrderedQueryable<Material>>>()
      {
        [nameof(ArticlesPresenter)] = x => x.OrderByDescending(y => y.LastActivity),
        [nameof(MaterialsPresenter)] = x => x.OrderByDescending(y => y.PublishDate),
        [nameof(BlogPresenter)] = x => x.OrderByDescending(y => y.LastActivity),
        [nameof(ForumPresenter)] = x => x.OrderByDescending(y => y.LastActivity)
      };
  }
}
