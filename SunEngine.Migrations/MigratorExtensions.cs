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

        /*public static ICreateTableWithColumnOrSchemaOrDescriptionSyntax SunTable(this ICreateExpressionRoot createExpressionRoot, string tableName)
        {
            return createExpressionRoot.Table(tableName.ToSnakeCase());
        }
        
        public static ICreateTableColumnAsTypeSyntax WithSunColumn(
            this ICreateTableWithColumnOrSchemaOrDescriptionSyntax createTableColumn, string columnName)
        {
            return createTableColumn.WithColumn(columnName.ToSnakeCase());
        }

        public static ICreateTableColumnAsTypeSyntax WithSunColumn(
            this ICreateTableColumnOptionOrWithColumnSyntax createTableColumn, string columnName)
        {
            return createTableColumn.WithColumn(columnName.ToSnakeCase());
        }

        public static IAlterTableColumnAsTypeSyntax AddSunColumn(
            this IAlterTableAddColumnOrAlterColumnOrSchemaOrDescriptionSyntax table, string columnName)
        {
            return table.AddColumn(columnName.ToSnakeCase());
        }

        public static TNextFk ForeignSunKey<TNext, TNextFk>(
            this IColumnOptionSyntax<TNext, TNextFk> columnOptionSyntax,
            string foreignKeyName, string primaryTableName, string primaryColumnName) where TNext : IFluentSyntax
            where TNextFk : IFluentSyntax
        {
            return columnOptionSyntax.ForeignKey(foreignKeyName.ToLower(), primaryTableName.ToSnakeCase(),
                primaryColumnName.ToSnakeCase());
        }

        public static ICreateForeignKeyFromTableSyntax SunForeignKey(this ICreateExpressionRoot createExpressionRoot, string name)
        {
            return createExpressionRoot.ForeignKey(name.ToLower());
        }*/
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
