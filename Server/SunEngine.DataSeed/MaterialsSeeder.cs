using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AngleSharp.Extensions;
using AngleSharp.Parser.Html;
using SunEngine.Core.Models;
using SunEngine.Core.Models.Materials;
using SunEngine.Core.Services;
using SunEngine.Core.Utils;

namespace SunEngine.DataSeed
{
	/// <summary>
	/// Seed materials and comments to DataContainer for testing purposes 
	/// </summary>
	public class MaterialsSeeder
	{
		public struct ParagraphsCount
		{
			public int Min;
			public int Max;
		}

		private readonly DataContainer dataContainer;

		private readonly Random ran = new Random();

		public int MinMaterialCount = 5;
		public int MaxMaterialCount = 20;
		public int CommentsCount = 12;
		public bool TitleAppendCategoryName;

		private const int MaterialSubTitleLength = 80;

		protected List<string> titles;
		protected List<string> paragraphs;

		private readonly ParagraphsCount defaultMaterialParagraphsCount = new ParagraphsCount {Min = 2, Max = 8};
		private readonly ParagraphsCount defaultCommentsParagraphsCount = new ParagraphsCount {Min = 1, Max = 2};


		public MaterialsSeeder(DataContainer dataContainer, List<string> titles, List<string> paragraphs)
		{
			this.dataContainer = dataContainer;
			this.titles = titles;
			this.paragraphs = paragraphs;
		}

		public void SeedCategoryAndSub(string categoryName)
		{
			Console.ForegroundColor = ConsoleColor.Green;
			Console.WriteLine("Seed test materials and comments in memory");
			Console.ResetColor();

			SeedCategoryAndSubRec(categoryName);
		}

		public void SeedCategoryAndSubRec(string categoryName)
		{
			var category =
				dataContainer.Categories.FirstOrDefault(x => x.NameNormalized == Normalizer.Normalize(categoryName));
			if (category == null)
				throw new Exception($"No category '{categoryName}' in data base");


			if (category.IsMaterialsContainer)
				SeedCategoryWithMaterials(category, TitleAppendCategoryName);

			foreach (var subCategory in dataContainer.Categories.Where(x =>
				x.ParentId.HasValue && x.ParentId.Value == category.Id))
				SeedCategoryAndSubRec(subCategory.Name);
		}

		public void SeedCategoryWithMaterials(
			Category category,
			bool titleAppendCategoryName = false,
			int? materialsCount = null,
			ParagraphsCount? materialParagraphsCount = null,
			ParagraphsCount? commentParagraphsCount = null)
		{
			if (materialsCount == null)
				materialsCount = ran.Next(MinMaterialCount, MaxMaterialCount);
			if (materialsCount == 0)
				return;


			Console.WriteLine($"'{category.Name}' category with {materialsCount} mat, {CommentsCount} comm");

			if (materialParagraphsCount == null)
				materialParagraphsCount = defaultMaterialParagraphsCount;

			if (commentParagraphsCount == null)
				commentParagraphsCount = defaultCommentsParagraphsCount;

			for (int i = 1; i <= materialsCount; i++)
			{
				var title = GetRandomTitle();
				if (titleAppendCategoryName)
					title += $" ({category.Name})";

				SeedMaterial(category, title, CommentsCount, materialParagraphsCount.Value,
					commentParagraphsCount.Value);
			}
		}

		public string GetRandomTitle()
		{
			return titles[ran.Next(0, this.titles.Count - 1)];
		}

		public string GetRandomText(int minParagraphs, int maxParagraphs)
		{
			StringBuilder sb = new StringBuilder();
			int paragraphsCount = ran.Next(minParagraphs, maxParagraphs + 1);
			for (int i = 0; i < paragraphsCount; i++)
			{
				sb.AppendLine($"<p>{paragraphs[ran.Next(0, this.paragraphs.Count - 1)]}</p>");
			}

			return sb.ToString();
		}

		public Material SeedMaterial(
			Category category,
			string title,
			int commentsCount, ParagraphsCount materialParagraphsCount, ParagraphsCount commentsParagraphsCount)
		{
			var publishDate = dataContainer.IterateCommentPublishDate();

			int id = dataContainer.NextMaterialId();

			Material material = new Material
			{
				Id = id,
				Title = title,
				Text = GetRandomText(materialParagraphsCount.Min, materialParagraphsCount.Max),
				AuthorId = dataContainer.GetRandomUserId(),
				CategoryId = category.Id,
				PublishDate = publishDate,
				LastActivity = publishDate,
				SortNumber = id
			};

			if (commentsCount > 0)
			{
				var comments = MakeComments(material, commentsCount, commentsParagraphsCount);

				material.LastActivity = comments.OrderByDescending(x => x.PublishDate).First().PublishDate;
				material.CommentsCount = comments.Count;

				dataContainer.Comments.AddRange(comments);
			}

			dataContainer.Materials.Add(material);

			return material;
		}

		public IList<Comment> MakeComments(Material material, int commentsCount,
			ParagraphsCount commentsParagraphsCount)
		{
			var addedComments = new List<Comment>();

			for (int i = 1; i <= CommentsCount; i++)
			{
				var comment = new Comment
				{
					Id = dataContainer.NextCommentId(),
					Text = GetRandomText(commentsParagraphsCount.Min, commentsParagraphsCount.Max),
					PublishDate = dataContainer.IterateCommentPublishDate(),
					MaterialId = material.Id,
					AuthorId = dataContainer.GetRandomUserId()
				};

				dataContainer.IterateCommentPublishDate();

				addedComments.Add(comment);
			}

			return addedComments;
		}


		private string MakeSeedText(string str, int wordInLine, int lines, string firstLine = null)
		{
			StringBuilder sb = new StringBuilder();
			if (firstLine != null)
				sb.AppendLine($"<p>{firstLine}</p>");

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

			return new HtmlParser().Parse(sb.ToString()).Body.ChildNodes.ToHtml(Sanitizer.OutputFormatter);
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