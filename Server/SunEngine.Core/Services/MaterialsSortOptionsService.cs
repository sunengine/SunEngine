using System;
using System.Collections.Generic;
using System.Linq;
using SunEngine.Core.Models.Materials;

namespace SunEngine.Core.Presenters
{
  public static class MaterialsSortOptionsService
  {
    public static Dictionary<string, Func<IQueryable<Material>, IOrderedQueryable<Material>>> MaterialsSortOptions =>
      new Dictionary<string, Func<IQueryable<Material>, IOrderedQueryable<Material>>>()
      {
        ["SortNumber desc"] = x => x.OrderByDescending(x => x.SortNumber),
        ["SortNumber asc"] = x => x.OrderBy(x => x.SortNumber),
        ["PublishDate asc"] = x => x.OrderBy(x => x.PublishDate),
        ["PublishDate desc"] = x => x.OrderByDescending(x => x.PublishDate),
        ["LastCommentDate asc"] = x => x.OrderBy(x => x.LastActivity),
        ["LastCommentDate desc"] = x => x.OrderByDescending(x => x.LastActivity)
      };
  }
}
