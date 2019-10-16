using System;
using FluentMigrator.Builders;
using FluentMigrator.Builders.Alter.Table;
using FluentMigrator.Builders.Create;
using FluentMigrator.Builders.Create.ForeignKey;
using FluentMigrator.Builders.Create.Table;
using FluentMigrator.Infrastructure;

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
