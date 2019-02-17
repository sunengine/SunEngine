using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LinqToDB;
using LinqToDB.Data;
using SunEngine.DataBase;
using SunEngine.Models.Materials;
using SunEngine.Services;

namespace SunEngine.Managers
{
    public interface ITagsManager
    {
        Task<IList<Tag>> InsertTags(string tags);
        Task<IList<Tag>> InsertTags(IList<string> tags);
        Task MaterialCreateAndSetTagsAsync(Material material, string tags);
        Task MaterialSetTags(Material material, IEnumerable<Tag> tags);
    }

    public class TagsManager : DbService, ITagsManager
    {
        public TagsManager(DataBaseConnection db) : base(db)
        {
            
        }
        
        public virtual async Task<IList<Tag>> InsertTags(string tags)
        {
            if (tags == null)
            {
                tags = "";
            }
            
            var tagsList = tags.Split(',');

            return await InsertTags(tagsList);
        }

        public virtual async Task<IList<Tag>> InsertTags(IList<string> tags)
        {
            if(tags == null)
            {
                return new List<Tag>();
            }

            tags = tags.Select(x=>x.Trim().TrimStart('#').ToLower()).Distinct().ToList();
            tags.Remove("");

            List<string> tagsToInsert = tags.ToList();
            var tagsResult = await db.Tags.Where(x => tags.Contains(x.Name)).ToListAsync();
            tagsResult.ForEach(x=>tagsToInsert.Remove(x.Name));

            foreach (var tagName in tagsToInsert)
            {
                Tag tag = new Tag
                {
                    Name = tagName
                };
                tag.Id = await db.InsertWithInt32IdentityAsync(tag);
                tagsResult.Add(tag);
            }

            return tagsResult;
        }
        
        public virtual async Task MaterialCreateAndSetTagsAsync(Material material, string tags)
        {
            var tagsList = await InsertTags(tags);
            await MaterialSetTags(material,tagsList);
        }

        public virtual async Task MaterialSetTags(Material material, IEnumerable<Tag> tags)
        {
            // TODO make auto delete unused tags
            await db.TagMaterials.Where(x => x.MaterialId == material.Id).DeleteAsync();

            var materialTags = tags.Select(x => new TagMaterial {TagId = x.Id, MaterialId = material.Id}).ToList();

            db.TagMaterials.BulkCopy(materialTags);
        }
    }
}