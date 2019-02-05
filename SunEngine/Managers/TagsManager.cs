using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LinqToDB;
using LinqToDB.Data;
using SunEngine.Commons.DataBase;
using SunEngine.Commons.Models;

namespace SunEngine.EntityServices
{
    public class TagsManager : DbService
    {
        public TagsManager(DataBaseConnection db) : base(db)
        {
            
        }
        
        public async Task<IList<Tag>> InsertTags(string tags)
        {
            if (tags == null)
            {
                tags = "";
            }
            
            var tagsList = tags.Split(',').Select(x => x.Trim().TrimStart('#').ToLower()).ToList();
            tagsList = tagsList.Distinct().ToList();
            tagsList.Remove("");

            return await InsertTags(tagsList);
        }

        public async Task<IList<Tag>> InsertTags(IList<string> preparedTags)
        {
            if(preparedTags == null)
            {
                return new List<Tag>();
            }

            List<string> tagsToInsert = preparedTags.ToList();
            var tags = await db.Tags.Where(x => preparedTags.Contains(x.Name)).ToListAsync();
            tags.ForEach(x=>tagsToInsert.Remove(x.Name));

            foreach (var tagName in tagsToInsert)
            {
                Tag tag = new Tag
                {
                    Name = tagName
                };
                tag.Id = (int) await db.InsertWithInt32IdentityAsync(tag);
                tags.Add(tag);
            }

            return tags;
        }
        
        public async Task MaterialCreateAndSetTagsAsync(Material material, string tags)
        {
            var tagsList = await InsertTags(tags);
            await MaterialSetTags(material,tagsList);
        }

        public async Task MaterialSetTags(Material material, IList<Tag> tags)
        {
            // TODO make auto delete unused tags
            db.TagMaterials.Where(x => x.MaterialId == material.Id).Delete();

            var materialTags = tags.Select(x => new TagMaterial {TagId = x.Id, MaterialId = material.Id}).ToList();

            db.TagMaterials.BulkCopy(materialTags);
        }
    }
}