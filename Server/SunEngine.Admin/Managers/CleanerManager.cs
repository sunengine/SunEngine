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

        Task DeleteAllMarkedMaterials();
    }

    public class CleanerManager : ICleanerManager
    {
        private readonly DataBaseConnection db;

        private async Task DeleteIndex(Material m)
        {
            db.Materials.Set(m => m.LastCommentId, () => null);
        }

        public CleanerManager(
            DataBaseConnection dataBaseConnection)
        {
            db = dataBaseConnection;
        }

        public async Task DeleteMaterial(Material material)
        {
            throw new NotImplementedException();
            //var material = db.Materials.Where(x => x.Id == idMaterial);

            //await material.Select(x => x.Comments).DeleteAsync();
            //await material.DeleteAsync();
        }

        public async Task DeleteComment(Comment comment)
        {
            throw new NotImplementedException();
            //var comment = db.Comments.Where(x => x.Id == idComment).ElementAt(0);
            //var material = comment.Material;

            //if (material.LastCommentId == comment.Id)
            //{
            //    var newLastComment = material.Comments.OrderBy(x => x.PublishDate).Last();
            //    material.LastCommentId = newLastComment.Id;
            //    material.LastComment = newLastComment;

            //    await db.UpdateAsync(material);
            //}

            //await db.DeleteAsync(comment);
        }

        public async Task DeleteAllMarkedMaterials()
        {
            using (await db.BeginTransactionAsync())
            {
                var materials = db.Materials.Where(x => x.DeletedDate != null).AsQueryable();

                foreach (var m in materials)
                {
                    await DeleteIndex(m);
                }

                await materials.Select(x => x.Comments).DeleteAsync();

                await materials.DeleteAsync();

                var comments = db.Comments.Where(x => x.DeletedDate != null).AsQueryable();
                await comments.DeleteAsync();
                await db.CommitTransactionAsync();
            }
        }
    }
}
