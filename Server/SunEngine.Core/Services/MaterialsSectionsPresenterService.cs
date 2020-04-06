using System;
using System.Collections.Generic;
using SunEngine.Core.Presenters;

namespace SunEngine.Core.Services
{
  public static class MaterialsSectionsPresenterService
  {
    public static Dictionary<string, Type> MaterialsSectionsPresenters => new Dictionary<string, Type>()
    {
      [nameof(MaterialsPresenter)] = typeof(MaterialsPresenter),
      [nameof(ForumPresenter)] = typeof(ForumPresenter),
      [nameof(ArticlesPresenter)] = typeof(ArticlesPresenter),
      [nameof(BlogPresenter)] = typeof(BlogPresenter)
    }; 
  }
}
