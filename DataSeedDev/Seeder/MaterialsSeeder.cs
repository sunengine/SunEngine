using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LinqToDB;
using LinqToDB.Data;
using SunEngine.Commons.DataBase;
using SunEngine.Commons.Models;
using SunEngine.Commons.TextProcess;

namespace SunEngine.Seeder
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


        private readonly LinesCount defaultLinesCount = new LinesCount() {Min = 4, Max = 30};


        public MaterialsSeeder(DataContainer dataContainer)
        {
            this.dataContainer = dataContainer;
        }

        public void SeedMaterials(Category category, int? materialsCount = null, LinesCount? linesCount = null)
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
                SeedMaterial(category, $"Материал {i} ({category.Name})",
                    (i >= 2 && i <= 3) ? 0 : 12,
                    $"Материал {i}, категория {category.Name}", "материал " + i, linesCount.Value);
            }
        }

        public Material SeedMaterial(Category category, string title, int messagesCount, string firstLine,
            string lineElement, LinesCount linesCount)
        {
            var publishDate = dataContainer.IterateMessagePublishDate();
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

            material.MakePreviewAndDescription(MaterialDescriptionLength,MaterialPreviewLength);
            
            if (messagesCount > 0)
            {
                IList<Message> messages = MakeMessages(material, messagesCount);

                //Message last = messages.OrderByDescending(x=>x.PublishDate).First();
                //material.SetLastMessage(last);

                material.LastActivity = messages.OrderByDescending(x => x.PublishDate).First().PublishDate;
                material.MessagesCount = messages.Count;
                
                dataContainer.Messages.AddRange(messages);
            }

            dataContainer.Materials.Add(material);

            return material;
        }

        public IList<Message> MakeMessages(Material material, int messagesCount)
        {
            List<Message> addedMessages = new List<Message>();

            for (int i = 1; i < 12; i++)
            {
                Message message = new Message
                {
                    Id = dataContainer.NextMessageId(),
                    Text = "",
                    PublishDate = dataContainer.IterateMessagePublishDate(),
                    MaterialId = material.Id,
                    AuthorId = dataContainer.GetRandomUserId()
                };

                dataContainer.IterateMessagePublishDate();

                message.Text = MakeSeedText("сообщение " + i, 8, 4, $"Сообщение: {message.Id}, материал {material.Id}");

                addedMessages.Add(message);
            }

            return addedMessages;
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
}