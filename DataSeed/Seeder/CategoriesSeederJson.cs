using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Newtonsoft.Json.Linq;
using NJsonSchema.Infrastructure;
using SunEngine.Models;

namespace DataSeed.Seeder
{
    public class CategoriesSeederJson
    {
        private readonly DataContainer dataContainer;
        private readonly MaterialsSeeder materialsSeeder;

        public CategoriesSeederJson(DataContainer dataContainer,
            MaterialsSeeder materialsSeeder)
        {
            this.dataContainer = dataContainer;
            this.materialsSeeder = materialsSeeder;
        }

        public void Seed(string fileName)
        {
            string jsonText = File.ReadAllText(fileName);
            JArray categoriesJson = JArray.Parse(jsonText);

            int i = 1;
            foreach (var categoryJson in categoriesJson)
            {
                SeedCategory(dataContainer.RootCategory, categoryJson, new List<int> {i++});
            }
        }

        private readonly Regex regex = new Regex(@"\[n+\]");

        private string PrepareText(string text, IList<int> numbers)
        {
            if (text == null)
            {
                return null;
            }

            return regex.Replace(text, c =>
            {
                int num = c.Value.Length - 2;
                return numbers[num - 1].ToString();
            });
        }

        private void SeedCategory(Category parent, JToken categoryToken, IList<int> numbers)
        {
            int repeatCount = 1;
            var repeat = categoryToken["Repeat"];
            if (repeat != null)
            {
                repeatCount = (int) repeat;
            }

            for (int j = 0; j < repeatCount; j++)
            {
                int id = dataContainer.NextCategoryId();
                Category category = new Category
                {
                    Id = id,
                    ParentId = parent?.Id,
                    Name = PrepareText((string) categoryToken["Name"], numbers),
                    Title = PrepareText((string) categoryToken["Title"], numbers),
                    Header = PrepareText((string) categoryToken["Header"], numbers),
                    SortNumber = id
                };

                if (categoryToken["IsMaterialsContainer"] != null)
                {
                    category.IsMaterialsContainer = (bool) categoryToken["IsMaterialsContainer"];
                }

                var sectionTypeName = categoryToken["SectionType"];
                if (sectionTypeName != null)
                {
                    var sectionType =
                        dataContainer.SectionTypes.FirstOrDefault(x => x.Name == (string) sectionTypeName);
                    category.SectionTypeId = sectionType.Id;
                    category.SectionType = sectionType;
                    category.AppendUrlToken = true;
                }

                if (categoryToken["AppendUrlToken"] != null)
                {
                    category.AppendUrlToken = (bool) categoryToken["AppendUrlToken"];
                }

                dataContainer.Categories.Add(category);

                if (categoryToken["SubCategories"] != null)
                {
                    var numbers1 = new List<int> {1};
                    numbers1.AddRange(numbers);

                    foreach (JToken subCategoryToken in (JArray) categoryToken["SubCategories"])
                    {
                        SeedCategory(category, subCategoryToken, numbers1);
                    }
                }

                string linesCount = (string) categoryToken["MaterialLinesCount"];
                int minLinesCount;
                int maxLinesCount;

                if (linesCount != null)
                {
                    string[] lineCountArr = linesCount.Split("-");
                    minLinesCount = int.Parse(lineCountArr[0]);
                    maxLinesCount = int.Parse(lineCountArr[1]);
                }

                if (category.IsMaterialsContainer)
                {
                    if (categoryToken["MaterialsCount"] != null)
                    {
                        int materialsCount = (int) categoryToken["MaterialsCount"];
                        materialsSeeder.SeedMaterials(category, materialsCount);
                    }
                    else
                    {
                        materialsSeeder.SeedMaterials(category);
                    }
                }

                numbers[0]++;
            }
        }
    }
}