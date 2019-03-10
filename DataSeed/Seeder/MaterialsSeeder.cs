using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SunEngine.Models;
using SunEngine.Models.Materials;
using SunEngine.Utils.TextProcess;

namespace DataSeedDev.Seeder
{
    public class MaterialsSeeder
    {
        public struct LinesCount
        {
            public int Min;
            public int Max;
        }

        private readonly DataContainer dataContainer;

        private readonly Random _ran = new Random();

        private const int MinMaterialCount = 5;
        private const int MaxMaterialCount = 20;

        private const int MaterialDescriptionLength = 80;
        private const int MaterialPreviewLength = 800;


        private readonly LinesCount defaultLinesCount = new LinesCount {Min = 4, Max = 30};


        public MaterialsSeeder(DataContainer dataContainer)
        {
            this.dataContainer = dataContainer;
        }

        public void SeedMaterials(Category category, string titleStart = null, bool titleAppendCategoryName = true,
            int? materialsCount = null, LinesCount? linesCount = null)
        {
            if (materialsCount == null)
            {
                materialsCount = _ran.Next(MinMaterialCount, MaxMaterialCount);
            }

            if (linesCount == null)
            {
                linesCount = defaultLinesCount;
            }

            for (int i = 1; i <= materialsCount; i++)
            {
                var title = titleStart != null ? titleStart + " " + i : $"Материал {i}";
                if (titleAppendCategoryName)
                    title += " (" + category.Name + ")";

                SeedMaterial(category, title,
                    (i >= 2 && i <= 3) ? 0 : 12,
                    $"{titleStart ?? "Материал"} {i}, категория {category.Name}", "материал " + i, linesCount.Value);
            }
        }

        public Material SeedMaterial(Category category, string title, int commentsCount, string firstLine,
            string lineElement, LinesCount linesCount)
        {
            var publishDate = dataContainer.IterateCommentPublishDate();
            int linesCountCurrent = _ran.Next(linesCount.Min, linesCount.Max);

            Material material = new Material
            {
                Id = dataContainer.NextMaterialId(),
                Title = title,
                Text = MakeSeedText(lineElement, 8, linesCountCurrent, firstLine),
                AuthorId = dataContainer.GetRandomUserId(),
                CategoryId = category.Id,
                PublishDate = publishDate,
                LastActivity = publishDate
            };

            var (preview, description) = MaterialExtensions.MakePreviewAndDescription(material.Text,
                MaterialDescriptionLength,
                MaterialPreviewLength);

            material.Preview = preview;

            SectionType sectionType = category.GetSectionType();

            if (sectionType != null && sectionType.Name == SectionTypeNames.Articles)
                material.Description = "Описание материала: " + material.Title;
            else
                material.Description = description;


            if (commentsCount > 0)
            {
                IList<Comment> comments = MakeComments(material, commentsCount);

                //Comment last = comments.OrderByDescending(x=>x.PublishDate).First();
                //material.SetLastComment(last);

                material.LastActivity = comments.OrderByDescending(x => x.PublishDate).First().PublishDate;
                material.CommentsCount = comments.Count;

                dataContainer.Comments.AddRange(comments);
            }

            dataContainer.Materials.Add(material);

            return material;
        }

        public IList<Comment> MakeComments(Material material, int commentsCount)
        {
            List<Comment> addedComments = new List<Comment>();

            for (int i = 1; i < 12; i++)
            {
                Comment comment = new Comment
                {
                    Id = dataContainer.NextCommentId(),
                    Text = "",
                    PublishDate = dataContainer.IterateCommentPublishDate(),
                    MaterialId = material.Id,
                    AuthorId = dataContainer.GetRandomUserId()
                };

                dataContainer.IterateCommentPublishDate();

                comment.Text = MakeSeedText("комментарий " + i, 8, 4,
                    $"Комментарий: {comment.Id}, материал {material.Id}");

                addedComments.Add(comment);
            }

            return addedComments;
        }


        private string MakeSeedText(string str, int wordInLine, int lines, string firstLine = null)
        {
            StringBuilder sb = new StringBuilder();
            if (firstLine != null)
            {
                sb.AppendLine($"<p>{firstLine}</p>");
            }

            for (int i = 0; i < lines; i += 3)
            {
                sb.Append("<p>");

                for (int j = 0; j < 3; j++)
                {
                    for (int k = 0; k < wordInLine; k++)
                        sb.Append(str + " ");

                    if (j != 2)
                        sb.Append("<br/>");
                }

                sb.Append("</p>\n");
            }

            return sb.ToString();
        }
    }

    public static class CategoryExtensions
    {
        public static SectionType GetSectionType(this Category category)
        {
            Category current = category;
            while (current != null)
            {
                if (current.SectionType != null)
                    return current.SectionType;

                current = current.Parent;
            }

            return null;
        }
    }

    public static class MaterialExtension
    {
        public static void SetLastComment(this Material material, Comment comment)
        {
            if (comment != null)
            {
                material.LastCommentId = comment.Id;
                material.LastActivity = comment.PublishDate;
            }
        }
    }
}