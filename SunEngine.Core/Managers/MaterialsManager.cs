using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using LinqToDB;
using Microsoft.Extensions.Options;
using SunEngine.Core.Configuration.Options;
using SunEngine.Core.DataBase;
using SunEngine.Core.Errors;
using SunEngine.Core.Models.Materials;
using SunEngine.Core.Services;
using SunEngine.Core.Utils.TextProcess;


namespace SunEngine.Core.Managers
{
    public interface IMaterialsManager
    {
        Task<int?> GetCategoryIdAsync(int materialId);
        Task<int?> GetCategoryIdAsync(string materialName);
        Task<Material> GetAsync(int id);
        Task CreateAsync(Material material, string tags, bool isSubTitleEditable);
        Task UpdateAsync(Material material, string tags, bool isSubTitleEditable);

        /// <summary>
        /// Set IsDeleted = true
        /// </summary>
        Task DeleteAsync(Material material);

        /// <summary>
        /// Set IsDeleted = false
        /// </summary>
        Task RestoreAsync(Material material);

        Task DetectAndSetLastCommentAndCountAsync(int materialId);
        Task DetectAndSetLastCommentAndCountAsync(Material material);
        bool IsNameValid(string name);
        Task<bool> IsNameInDbAsync(string name);

        /// <summary>
        /// Move up in sort order (SortNumber field)
        /// </summary>
        Task UpAsync(int id);

        /// <summary>
        /// Move down in sort order (SortNumber field)
        /// </summary>
        Task DownAsync(int id);
    }

    public class MaterialsManager : DbService, IMaterialsManager
    {
        protected readonly ITagsManager tagsManager;
        protected readonly Sanitizer sanitizer;
        protected readonly MaterialsOptions materialsOptions;

        protected readonly Regex nameValidator =
            new Regex("^[a-zA-Z0-9-]{3," + DbColumnSizes.Materials_Name + "}$");

        public MaterialsManager(
            DataBaseConnection db,
            Sanitizer sanitizer,
            ITagsManager tagsManager,
            IOptions<MaterialsOptions> materialsOptions) : base(db)
        {
            this.tagsManager = tagsManager;
            this.sanitizer = sanitizer;
            this.materialsOptions = materialsOptions.Value;
        }


        public virtual async Task<int?> GetCategoryIdAsync(int materialId)
        {
            return await db.Materials.Where(x => x.Id == materialId).Select(x => x.Category.Id)
                .FirstOrDefaultAsync();
        }

        public virtual async Task<int?> GetCategoryIdAsync(string materialName)
        {
            return await db.Materials.Where(x => x.Name == materialName).Select(x => x.Category.Id)
                .FirstOrDefaultAsync();
        }

        public virtual Task<Material> GetAsync(int id)
        {
            return db.Materials.FirstOrDefaultAsync(x => x.Id == id);
        }

        public virtual async Task CreateAsync(
            Material material, string tags, bool isSubTitleEditable = false)
        {
            material.Text = sanitizer.Sanitize(material.Text);

            var (preview, subTitle) = MaterialExtensions.MakePreviewAndSubTitle(
                material.Text,
                materialsOptions.SubTitleLength,
                materialsOptions.PreviewLength);

            material.Preview = preview;

            material.SubTitle = isSubTitleEditable ? 
                SimpleHtmlToText.ClearTags(sanitizer.Sanitize(material.SubTitle)) : 
                subTitle;

            using (db.BeginTransaction())
            {
                material.Id = await db.InsertWithInt32IdentityAsync(material);
                await db.Materials.Where(x => x.Id == material.Id).Set(x => x.SortNumber, x => material.Id)
                    .UpdateAsync();

                await tagsManager.MaterialCreateAndSetTagsAsync(material, tags);
                db.CommitTransaction();
            }
        }

        public virtual async Task UpdateAsync(
            Material material, string tags, bool isSubTitleEditable = false)
        {
            material.Text = sanitizer.Sanitize(material.Text);

            var (preview, subTitle) = MaterialExtensions.MakePreviewAndSubTitle(material.Text,
                materialsOptions.SubTitleLength,
                materialsOptions.PreviewLength);

            material.Preview = preview;

            material.SubTitle = isSubTitleEditable ? 
                SimpleHtmlToText.ClearTags(sanitizer.Sanitize(material.SubTitle)) : 
                subTitle;

            await db.UpdateAsync(material);

            await tagsManager.MaterialCreateAndSetTagsAsync(material, tags);
        }

        public virtual async Task DeleteAsync(Material material)
        {
            await db.Materials.Where(x => x.Id == material.Id).Set(x => x.IsDeleted, true).UpdateAsync();
        }

        public virtual async Task RestoreAsync(Material material)
        {
            await db.Materials.Where(x => x.Id == material.Id).Set(x => x.IsDeleted, false)
                .UpdateAsync();
        }

        public virtual bool IsNameValid(string name)
        {
            if (int.TryParse(name, out _))
                return false;
            return nameValidator.IsMatch(name);
        }

        public virtual Task<bool> IsNameInDbAsync(string name)
        {
            return db.Materials.AnyAsync(x => x.Name.ToLower() == name.ToLower());
        }

        public async Task UpAsync(int id)
        {
            var material = await db.Materials.Where(x => !x.IsDeleted)
                .FirstOrDefaultAsync(x => x.Id == id);
            if (material == null)
                throw new SunEntityNotFoundException(nameof(Material), id);

            var material2 = await db.Materials
                .Where(x =>
                    !x.IsDeleted && x.CategoryId == material.CategoryId &&
                    x.SortNumber > material.SortNumber)
                .OrderBy(x => x.SortNumber).FirstOrDefaultAsync();

            if (material2 == null)
                throw new SunEntityNotFoundException(nameof(Material), "Previous material not found");

            db.BeginTransaction();
            await db.Materials.Where(x => x.Id == material.Id)
                .Set(x => x.SortNumber, material2.SortNumber)
                .UpdateAsync();
            await db.Materials.Where(x => x.Id == material2.Id)
                .Set(x => x.SortNumber, material.SortNumber)
                .UpdateAsync();
            db.CommitTransaction();
        }

        public async Task DownAsync(int id)
        {
            var material = await db.Materials.Where(x => !x.IsDeleted)
                .FirstOrDefaultAsync(x => x.Id == id);
            if (material == null)
                throw new SunEntityNotFoundException(nameof(Material), id);

            var material2 = await db.Materials
                .Where(x =>
                    !x.IsDeleted && x.CategoryId == material.CategoryId &&
                    x.SortNumber < material.SortNumber)
                .OrderByDescending(x => x.SortNumber).FirstOrDefaultAsync();

            if (material2 == null)
                throw new SunEntityNotFoundException(nameof(Material), "Next material not found");

            db.BeginTransaction();
            await db.Materials.Where(x => x.Id == material.Id)
                .Set(x => x.SortNumber, material2.SortNumber)
                .UpdateAsync();
            await db.Materials.Where(x => x.Id == material2.Id)
                .Set(x => x.SortNumber, material.SortNumber)
                .UpdateAsync();
            db.CommitTransaction();
        }

        public virtual Task DetectAndSetLastCommentAndCountAsync(int materialId)
        {
            return DetectAndSetLastCommentAndCountAsync(
                db.Materials.FirstOrDefault(x => x.Id == materialId));
        }

        public virtual async Task DetectAndSetLastCommentAndCountAsync(Material material)
        {
            var commentsQuery = db.Comments.Where(x => x.MaterialId == material.Id && !x.IsDeleted);

            var lastComment = await commentsQuery.OrderByDescending(x => x.PublishDate)
                .FirstOrDefaultAsync();
            var lastCommentId = lastComment?.Id;
            var lastActivity = lastComment?.PublishDate ?? material.PublishDate;

            var commentsCount = await commentsQuery.CountAsync();

            await db.Materials.Where(x => x.Id == material.Id)
                .Set(x => x.CommentsCount, x => commentsCount)
                .Set(x => x.LastCommentId, x => lastCommentId)
                .Set(x => x.LastActivity, x => lastActivity)
                .UpdateAsync();
        }
    }
}
