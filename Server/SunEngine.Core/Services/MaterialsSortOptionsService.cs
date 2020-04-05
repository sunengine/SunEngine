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
        ["SortNumber desc"] = x => x.OrderByDescending(y => y.SortNumber),
        ["SortNumber asc"] = x => x.OrderBy(y => y.SortNumber),
        ["PublishDate asc"] = x => x.OrderBy(y => y.PublishDate),
        ["PublishDate desc"] = x => x.OrderByDescending(y => y.PublishDate),
        ["LastCommentDate asc"] = x => x.OrderBy(y => y.LastActivity),
        ["LastCommentDate desc"] = x => x.OrderByDescending(y => y.LastActivity)
      };
  }
}
