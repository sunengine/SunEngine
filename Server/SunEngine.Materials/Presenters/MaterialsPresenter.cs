using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LinqToDB;
using SunEngine.Core.DataBase;
using SunEngine.Core.Models;
using SunEngine.Core.Models.Materials;
using SunEngine.Core.Services;
using SunEngine.Materials.Services;

namespace SunEngine.Materials.Presenters
{

  public interface IMaterialsQueryPresenter
  {
    Task<IList<Object>> GetMaterialsByCategoryAsync(MaterialsShowOptions options);
    Task<IList<Object>> GetMaterialsFromMultiCategoryAsync(MaterialsShowOptions options);
  }
  
	public interface IMaterialsPresenter
	{
		Task<MaterialView> GetAsync(int id);
		Task<MaterialView> GetAsync(string name);
  }

	public class MaterialsPresenter : DbService, IMaterialsPresenter, IMaterialsQueryPresenter
	{
		public MaterialsPresenter(DataBaseConnection db) : base(db)
		{
		}

		public Task<MaterialView> GetAsync(int id)
		{
			var query = db.Materials.Where(x => x.Id == id);
			return GetAsync(query);
		}

		public Task<MaterialView> GetAsync(string name)
		{
			var query = db.Materials.Where(x => x.Name == name);
			return GetAsync(query);
		}

    public Task<IList<Material>> GetMaterialsFromMultiCategory(IEnumerable<int> categoryNames)
    {
      throw new NotImplementedException();
    }


    protected virtual Task<MaterialView> GetAsync(IQueryable<Material> query)
		{
			return query.Select(x =>
				new MaterialView
				{
					Id = x.Id,
					Name = x.Name,
					Title = x.Title,
					SubTitle = x.SubTitle,
					AuthorLink = x.Author.Link,
					AuthorName = x.Author.UserName,
					AuthorAvatar = x.Author.Avatar,
					AuthorId = x.Author.Id,
					PublishDate = x.PublishDate,
					EditDate = x.EditDate,
					CommentsCount = x.CommentsCount,
					Text = x.Text,
					CategoryName = x.Category.Name,
					IsHidden = x.IsHidden,
					IsCommentsBlocked = x.IsCommentsBlocked,
					DeletedDate = x.DeletedDate,
					Tags = x.TagMaterials.OrderBy(y => y.Tag.Name).Select(y => y.Tag.Name).ToArray(),
					VisitsCount = x.VisitsCount,
					SettingsJson = x.SettingsJson
				}
			).FirstOrDefaultAsync();
		}
    
    public async Task<IList<Object>> GetMaterialsByCategoryAsync(MaterialsShowOptions materialsesShowOptions)
    {
      Func<IQueryable<Material>, IOrderedQueryable<Material>> orderBy;

      if (materialsesShowOptions.Sort != null)
        orderBy = materialsesShowOptions.Sort;
      else
        orderBy = MaterialsDefaultSortService.DefaultSortOptions.GetValueOrDefault(nameof(MaterialsPresenter));
      
      IQueryable<Material> materials = db.Materials;
      var res = await (from material in materials
        join category in db.GetTable<Category>() on material.CategoryId equals category.Id
        orderby orderBy
        where category.Id == materialsesShowOptions.CategoryId
        select new MaterialView()
        {
          Id = material.Id,
          Name = material.Name,
          Title = material.Title,
          SubTitle = material.SubTitle,
          AuthorLink = material.Author.Link,
          AuthorName = material.Author.UserName,
          AuthorAvatar = material.Author.Avatar,
          AuthorId = material.Author.Id,
          PublishDate = material.PublishDate,
          EditDate = material.EditDate,
          CommentsCount = material.CommentsCount,
          Text = material.Text,
          CategoryName = material.Category.Name,
          IsHidden = material.IsHidden,
          IsCommentsBlocked = material.IsCommentsBlocked,
          DeletedDate = material.DeletedDate,
          Tags = material.TagMaterials.OrderBy(y => y.Tag.Name).Select(y => y.Tag.Name).ToArray(),
          VisitsCount = material.VisitsCount,
          SettingsJson = material.SettingsJson
        }).ToListAsync();
      return res as IList<object>;
    }

    public async Task<IList<object>> GetMaterialsFromMultiCategoryAsync(MaterialsShowOptions options)
    {
      Func<IQueryable<Material>, IOrderedQueryable<Material>> orderBy;

      if (options.Sort != null)
        orderBy = options.Sort;
      else
        orderBy = MaterialsDefaultSortService.DefaultSortOptions.GetValueOrDefault(nameof(MaterialsPresenter));
      
      IQueryable<Material> materials = db.Materials;

      var res = await (from material in materials
        join category in db.GetTable<Category>() on material.CategoryId equals category.Id
        orderby orderBy
        where options.CategoriesIds.Contains(material.CategoryId)
        select new MaterialView()
        {
          Id = material.Id,
          Name = material.Name,
          Title = material.Title,
          SubTitle = material.SubTitle,
          AuthorLink = material.Author.Link,
          AuthorName = material.Author.UserName,
          AuthorAvatar = material.Author.Avatar,
          AuthorId = material.Author.Id,
          PublishDate = material.PublishDate,
          EditDate = material.EditDate,
          CommentsCount = material.CommentsCount,
          Text = material.Text,
          CategoryName = material.Category.Name,
          IsHidden = material.IsHidden,
          IsCommentsBlocked = material.IsCommentsBlocked,
          DeletedDate = material.DeletedDate,
          Tags = material.TagMaterials.OrderBy(y => y.Tag.Name).Select(y => y.Tag.Name).ToArray(),
          VisitsCount = material.VisitsCount,
          SettingsJson = material.SettingsJson
        }).ToListAsync();

      return res as IList<object>;
    }
  }

	public class MaterialView
	{
		public string Name { get; set; }
		public int Id { get; set; }
		public string Title { get; set; }
		public string SubTitle { get; set; }
		public string Text { get; set; }
		public string AuthorName { get; set; }
		public int AuthorId { get; set; }
		public string AuthorLink { get; set; }
		public string AuthorAvatar { get; set; }
		public int CommentsCount { get; set; }
		public DateTime PublishDate { get; set; }
		public DateTime? EditDate { get; set; }
		public string CategoryName { get; set; }
		public bool IsCommentsBlocked { get; set; }
		public bool IsHidden { get; set; }
		public DateTime? DeletedDate { get; set; }
		public string[] Tags { get; set; }
		public int VisitsCount { get; set; }
		public string SettingsJson { get; set; }
	}
}
