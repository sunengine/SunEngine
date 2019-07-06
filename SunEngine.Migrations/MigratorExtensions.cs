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
}
