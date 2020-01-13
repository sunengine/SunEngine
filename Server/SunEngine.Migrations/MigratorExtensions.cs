using System;
using FluentMigrator.Builders.Create.Table;

namespace SunEngine.Migrations
{
	internal static class MigratorExtensions
	{
		public static ICreateTableColumnOptionOrWithColumnSyntax AsMaxString(
			this ICreateTableColumnAsTypeSyntax createTableColumnAsTypeSyntax)
		{
			return createTableColumnAsTypeSyntax.AsString(Int32.MaxValue);
		}
	}

	public static class StringExtension
	{
		public static string s(this string str)
		{
			return str;
			//return System.Text.RegularExpressions.Regex.Replace(str, "(?<=.)([A-Z])", "_$0",
			//    System.Text.RegularExpressions.RegexOptions.Compiled);
		}
	}
}