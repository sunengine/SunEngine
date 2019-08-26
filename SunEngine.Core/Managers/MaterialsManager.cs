using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AngleSharp.Dom.Html;
using AngleSharp.Parser.Html;
using LinqToDB;
using Microsoft.Extensions.Options;
using SunEngine.Core.Cache.CacheModels;
using SunEngine.Core.Cache.Services;
using SunEngine.Core.Configuration.Options;
using SunEngine.Core.DataBase;
using SunEngine.Core.Errors;
using SunEngine.Core.Models;
using SunEngine.Core.Models.Materials;
using SunEngine.Core.Services;
using SunEngine.Core.Utils;
using SunEngine.Core.Utils.TextProcess;


namespace SunEngine.Core.Managers
{
    public interface IMaterialsManager
    {
        ValueTask<int?> GetCategoryIdAsync(int materialId);
        ValueTask<int?> GetCategoryIdAsync(string materialName);
        Task<Material> GetAsync(int id);
        ValueTask CreateAsync(Material material, string tags, CategoryCached category);
        ValueTask UpdateAsync(Material material, string tags, CategoryCached category);

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
        ValueTask UpAsync(int id);

        /// <summary>
        /// Move down in sort order (SortNumber field)
        /// </summary>
        ValueTask DownAsync(int id);
    }

    public class MaterialsManager : DbService, IMaterialsManager
    {
        protected readonly ITagsManager tagsManager;
        protected readonly SanitizerService sanitizerService;
        protected readonly MaterialsOptions materialsOptions;
        protected readonly ICategoriesCache categoriesCache;

        protected readonly Regex nameValidator =
            new Regex("^[a-zA-Z0-9-]{3," + DbColumnSizes.Materials_Name + "}$");

        public MaterialsManager(
            DataBaseConnection db,
            SanitizerService sanitizerService,
            ICategoriesCache categoriesCache,
            ITagsManager tagsManager,
            IOptions<MaterialsOptions> materialsOptions) : base(db)
        {
            this.tagsManager = tagsManager;
            this.sanitizerService = sanitizerService;
            this.materialsOptions = materialsOptions.Value;
            this.categoriesCache = categoriesCache;
        }


        public virtual async ValueTask<int?> GetCategoryIdAsync(int materialId)
        {
            return await db.Materials.Where(x => x.Id == materialId).Select(x => x.Category.Id)
                .FirstOrDefaultAsync();
        }

        public virtual async ValueTask<int?> GetCategoryIdAsync(string materialName)
        {
            return await db.Materials.Where(x => x.Name == materialName).Select(x => x.Category.Id)
                .FirstOrDefaultAsync();
        }

        public virtual Task<Material> GetAsync(int id)
        {
            return db.Materials.FirstOrDefaultAsync(x => x.Id == id);
        }

        public virtual async ValueTask CreateAsync(Material material, string tags, CategoryCached category)
        {
            IHtmlDocument doc = new HtmlParser().Parse(material.Text);

            material.Text = sanitizerService.Sanitize(doc);
            material.SettingsJson = material.SettingsJson?.MakeJsonText();

            var generator = categoriesCache.GetMaterialsPreviewGenerator(category.MaterialsPreviewGeneratorName);
            material.Preview = generator(doc, materialsOptions.PreviewLength);


            switch (category.MaterialsSubTitleInputType)
            {
                case MaterialsSubTitleInputType.Manual:
                    material.SubTitle = SimpleHtmlToText.ClearTags(sanitizerService.Sanitize(material.SubTitle));
                    break;
                case MaterialsSubTitleInputType.Auto:
                    material.SubTitle = MakeSubTitle.Do(doc, materialsOptions.SubTitleLength);
                    break;
            }


            using (db.BeginTransaction())
            {
                material.Id = await db.InsertWithInt32IdentityAsync(material);
                await db.Materials.Where(x => x.Id == material.Id).Set(x => x.SortNumber, x => material.Id)
                    .UpdateAsync();

                await tagsManager.MaterialCreateAndSetTagsAsync(material, tags);
                db.CommitTransaction();
            }
        }

        public virtual async ValueTask UpdateAsync(
            Material material,
            string tags,
            CategoryCached category)
        {
            IHtmlDocument doc = new HtmlParser().Parse(material.Text);

            material.Text = sanitizerService.Sanitize(doc);
            material.SettingsJson = material.SettingsJson?.MakeJsonText();

            var generator = categoriesCache.GetMaterialsPreviewGenerator(category.MaterialsPreviewGeneratorName);
            material.Preview = generator(doc, materialsOptions.PreviewLength);


            switch (category.MaterialsSubTitleInputType)
            {
                case MaterialsSubTitleInputType.Manual:
                    material.SubTitle = SimpleHtmlToText.ClearTags(sanitizerService.Sanitize(material.SubTitle));
                    break;
                case MaterialsSubTitleInputType.Auto:
                    material.SubTitle = MakeSubTitle.Do(doc, materialsOptions.SubTitleLength);
                    break;
                default:
                    material.SubTitle = null;
                    break;
            }


            await db.UpdateAsync(material);

            await tagsManager.MaterialCreateAndSetTagsAsync(material, tags);
        }

        public virtual Task DeleteAsync(Material material)
        {
            return db.Materials.Where(x => x.Id == material.Id).Set(x => x.DeletedDate, DateTime.UtcNow).UpdateAsync();
        }

        public virtual Task RestoreAsync(Material material)
        {
            return db.Materials.Where(x => x.Id == material.Id).Set(x => x.DeletedDate, x => null)
                .UpdateAsync();
        }

        public virtual bool IsNameValid(string name)
        {
            return !int.TryParse(name, out _) && nameValidator.IsMatch(name);
        }

        public virtual Task<bool> IsNameInDbAsync(string name)
        {
            return db.Materials.AnyAsync(x => string.Equals(x.Name, name, StringComparison.CurrentCultureIgnoreCase));
        }

        public async ValueTask UpAsync(int id)
        {
            var material = await db.Materials.Where(x => x.DeletedDate == null)
                .FirstOrDefaultAsync(x => x.Id == id);
            if (material == null)
                throw new SunEntityNotFoundException(nameof(Material), id);

            var material2 = await db.Materials
                .Where(x =>
                    x.DeletedDate == null && x.CategoryId == material.CategoryId &&
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

        public async ValueTask DownAsync(int id)
        {
            var material = await db.Materials.Where(x => x.DeletedDate == null)
                .FirstOrDefaultAsync(x => x.Id == id);
            if (material == null)
                throw new SunEntityNotFoundException(nameof(Material), id);

            var material2 = await db.Materials
                .Where(x =>
                    x.DeletedDate == null && x.CategoryId == material.CategoryId &&
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
            return DetectAndSetLastCommentAndCountAsync(db.Materials.FirstOrDefault(x => x.Id == materialId));
        }

        public virtual async Task DetectAndSetLastCommentAndCountAsync(Material material)
        {
            var commentsQuery = db.Comments.Where(x => x.MaterialId == material.Id && x.DeletedDate == null);

            var lastComment = await commentsQuery.OrderByDescending(x => x.PublishDate).FirstOrDefaultAsync();
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
