using System;
using System.Linq;
using System.Threading.Tasks;
using LinqToDB;
using SunEngine.Core.DataBase;
using SunEngine.Core.Models.Materials;

namespace SunEngine.Admin.Services
{
	public interface ICleanerManager
	{
		Task DeleteMaterial(Material material);

		Task DeleteComment(Comment comment);

		Task<DeletedCleanerCount> DeleteAllMarkedMaterials();
	}

	public class CleanerManager : ICleanerManager
	{
		private readonly DataBaseConnection db;

		public CleanerManager(
			DataBaseConnection dataBaseConnection)
		{
			db = dataBaseConnection;
		}

		public Task DeleteMaterial(Material material)
		{
			throw new NotImplementedException();
		}

		public Task DeleteComment(Comment comment)
		{
			throw new NotImplementedException();
		}

		public async Task<DeletedCleanerCount> DeleteAllMarkedMaterials()
		{
			int deletedMaterialCount = 0;
			int deletedCommentCount = 0;

			using (await db.BeginTransactionAsync())
			{
				var materials = db.Materials.Where(x => x.DeletedDate != null);

				await materials.Set(x => x.LastCommentId, (x) => null).UpdateAsync();

				deletedCommentCount += await db.Comments.Where(x => x.Material.DeletedDate != null).DeleteAsync();
				deletedMaterialCount += await materials.DeleteAsync();

				deletedCommentCount += await db.Comments.Where(x => x.DeletedDate != null).AsQueryable().DeleteAsync();
				await db.CommitTransactionAsync();
			}

			return new DeletedCleanerCount
				{DeletedMaterials = deletedMaterialCount, DeletedComments = deletedCommentCount};
		}
	}

	public class DeletedCleanerCount
	{
		public int DeletedMaterials { get; set; }

		public int DeletedComments { get; set; }
	}
}