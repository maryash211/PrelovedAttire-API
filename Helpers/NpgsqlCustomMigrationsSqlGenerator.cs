using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Infrastructure;
//using Microsoft.EntityFrameworkCore.Migrations.Design;
//using Microsoft.EntityFrameworkCore.Migrations.Sql;
using Microsoft.EntityFrameworkCore.Migrations.Internal;
using Npgsql.EntityFrameworkCore.PostgreSQL.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Infrastructure.Internal;

namespace GradProject_API.Helpers
{
    public class NpgsqlCustomMigrationsSqlGenerator : NpgsqlMigrationsSqlGenerator
    {
        public NpgsqlCustomMigrationsSqlGenerator(
            MigrationsSqlGeneratorDependencies dependencies,
            INpgsqlSingletonOptions npgsqlSingletonOptions)
            : base(dependencies, npgsqlSingletonOptions)
        {
        }

        protected override void ColumnDefinition(
            string? schema,
            string table,
            string name,
            ColumnOperation operation,
            IModel? model,
            MigrationCommandListBuilder builder)
        {
            // Convert 'nvarchar(max)' to 'text'
            if (operation.ColumnType == "nvarchar(max)")
            {
                operation.ColumnType = "text";
            }

            base.ColumnDefinition(schema, table, name, operation, model, builder);
        }
    }
}
