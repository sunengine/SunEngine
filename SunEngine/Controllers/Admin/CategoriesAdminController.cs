using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LinqToDB;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SunEngine.Commons.DataBase;
using SunEngine.Commons.Models;

namespace SunEngine.Controllers.Admin
{
    //[Authorize(Roles = "Admin")]
    public class CategoriesAdminController : BaseController
    {
        private readonly DataBaseConnection db;

        public CategoriesAdminController(DataBaseConnection db, UserManager<User> userManager) : base(userManager)
        {
            this.db = db;
        }

        [HttpPost]
        public async Task<IActionResult> GetAllCategories()
        {
            var categories = await db.Categories.OrderBy(x=>x.SortNumber).Select(x => new CategoryAdminViewModel
            {
                Id = x.Id,
                Name = x.Name,
                Title = x.Title,
                ChildrenType = x.ChildrenType,
                Description = x.Description,
                Header = x.Header,
                AreaRoot = x.AreaRoot,
                ParentId = x.ParentId,
                SortNumber = x.SortNumber,
                MaterialsCount = x.Materials.Count,
                IsHidden = x.IsHidden,
                IsDeleted = x.IsDeleted
            }).ToDictionaryAsync(x => x.Id);

            CategoryAdminViewModel root = null;

            foreach (var category in categories.Values)
            {
                if (!category.ParentId.HasValue)
                {
                    root = category;
                    continue; 
                }

                var parent = categories[category.ParentId.Value];
                if (parent.SubCategories == null) parent.SubCategories = new List<CategoryAdminViewModel>();

                parent.SubCategories.Add(category);
            }

            return Json(root);
        }
    }

    public class CategoryAdminViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Title { get; set; }

        public ChildrenType ChildrenType { get; set; }

        public string Description { get; set; }

        public string Header { get; set; }

        public bool AreaRoot { get; set; }

        public int? ParentId { get; set; }

        public IList<CategoryAdminViewModel> SubCategories { get; set; }

        public int SortNumber { get; set; }

        public int MaterialsCount { get; set; }

        public bool IsDeleted { get; set; }

        public bool IsHidden { get; set; }
    }
}