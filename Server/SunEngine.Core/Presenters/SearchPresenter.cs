using System;
using System.Linq;
using System.Threading.Tasks;
using LinqToDB;
using LinqToDB.DataProvider.MySql;
using SunEngine.Core.DataBase;
using SunEngine.Core.Errors.Exceptions;
using SunEngine.Core.Services;
using SunEngine.Core.Utils.TextProcess;

namespace SunEngine.Core.Presenters
{
	public interface ISearchPresenter
	{
		Task<UserInfoView[]> SearchByUsernameOrLink(string searchString);
		Task<MaterialSearchInfoView[]> SearchByMaterials(string searchString);
	}

	interface ISqlSearchRequest
	{
		UserInfoView[] SearchRequser(string searchString);
	}

	public class SearchPresenter : DbService, ISearchPresenter
	{
		public SearchPresenter(DataBaseConnection db) : base(db)
		{
		}

		public Task<UserInfoView[]> SearchByUsernameOrLink(string searchString)
		{
			return db.Users.Where(x => x.UserName.Contains(searchString, StringComparison.OrdinalIgnoreCase) ||
			                           x.Link.Contains(searchString, StringComparison.OrdinalIgnoreCase))
				.Select(x => new UserInfoView
				{
					Avatar = x.Avatar,
					Id = x.Id,
					Link = x.Link,
					Name = x.UserName
				}).ToArrayAsync();
		}

		public Task<MaterialSearchInfoView[]> SearchByMaterials(string searchString)
		{
			switch (db.SqlProviderName)
			{
				case SqlProviderType.MySql:
					return searchMaterialsMySql(searchString);
				case SqlProviderType.PostgreSql | SqlProviderType.Other:
					return searchMaterialsDefault(searchString);
				default:
					throw new SunDataBaseException("Database definition error");
			}
		}

		private Task<MaterialSearchInfoView[]> searchMaterialsMySql(string searchString)
		{
			return db.Materials.Where(x => Sql.Ext.MySql().Match(searchString, x.Title, x.Text))
				.OrderByDescending(x => x.Id)
				.Select(x => new MaterialSearchInfoView()
				{
					Id = x.Id,
					Title = x.Title,
					Text = SimpleHtmlToText.ClearTags(x.Text)
				}).ToArrayAsync();
		}

		private Task<MaterialSearchInfoView[]> searchMaterialsDefault(string searchString)
		{
			return db.Materials.Where(x => x.Text.Contains(searchString, StringComparison.OrdinalIgnoreCase) ||
			                               x.Text.Contains(searchString, StringComparison.OrdinalIgnoreCase))
				.OrderByDescending(x => x.Id)
				.Select(x => new MaterialSearchInfoView()
				{
					Id = x.Id,
					Title = x.Title,
					Text = x.Text
				}).ToArrayAsync();
		}
	}

	public class MaterialSearchInfoView
	{
		public int Id { get; set; }
		public string Title { get; set; }
		public string Text { get; set; }
	}

	public class UserInfoView
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string Link { get; set; }
		public string Avatar { get; set; }
	}
}