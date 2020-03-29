using System;
using System.Collections.Generic;
using SunEngine.Core.Presenters;

namespace SunEngine.Core.Services
{
  public class MaterialsSectionsPresenterService
  {
    public Dictionary<string, Type> MaterialsSectionsPresenters => new Dictionary<string, Type>()
    {
      ["materialPresenter"] = typeof(MaterialsPresenter),
      ["forumPresenter"] = typeof(ForumPresenter),
      ["articlePresenter"] = typeof(ArticlesPresenter),
      ["blogPresenter"] = typeof(BlogPresenter)
    }; 
  }
}
