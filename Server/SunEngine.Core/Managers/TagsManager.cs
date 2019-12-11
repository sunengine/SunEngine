using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LinqToDB;
using LinqToDB.Data;
using SunEngine.Core.DataBase;
using SunEngine.Core.Models.Materials;
using SunEngine.Core.Services;

namespace SunEngine.Core.Managers
{
    public interface ITagsManager
    {
        ValueTask<IList<Tag>> InsertTags(string tags);
        ValueTask<IList<Tag>> InsertTags(IList<string> tags);
        ValueTask MaterialCreateAndSetTagsAsync(Material material, string tags);
        ValueTask MaterialSetTags(Material material, IEnumerable<Tag> tags);
    }

    public class TagsManager : DbService, ITagsManager
    {
        public TagsManager(DataBaseConnection db) : base(db)
        {
            
        }
        
        public virtual async ValueTask<IList<Tag>> InsertTags(string tags)
        {
            if (tags == null)
            {
                tags = "";
            }
            
            var tagsList = tags.Split(',');

            return await InsertTags(tagsList);
        }

        public virtual async ValueTask<IList<Tag>> InsertTags(IList<string> tags)
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
        
        public virtual async ValueTask MaterialCreateAndSetTagsAsync(Material material, string tags)
        {
            var tagsList = await InsertTags(tags);
            await MaterialSetTags(material,tagsList);
        }

        public virtual async ValueTask MaterialSetTags(Material material, IEnumerable<Tag> tags)
        {
            // TODO make auto delete unused tags
            await db.TagMaterials.Where(x => x.MaterialId == material.Id).DeleteAsync();

            var materialTags = tags.Select(x => new TagMaterial {TagId = x.Id, MaterialId = material.Id}).ToList();

            db.TagMaterials.BulkCopy(materialTags);
        }
    }
}
