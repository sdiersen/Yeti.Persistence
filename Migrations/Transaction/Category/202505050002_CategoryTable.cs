
using FluentMigrator;

using Persistence.Migrations.Constants;

namespace Persistence.Migrations.Transaction;

[Migration(202505050002)]
public class AddUnAttachedCategoryTableRow : Migration
{
    public override void Up()
    {
        Insert.IntoTable($"{DbTableNames.CATEGORY_TABLE}")
            .Row(new
            {
                Name = "Unattached",
                Description = "This is a category for entries that are not attached to any category. Typically used as a temporary holding area until a proper category is created.",
                CreatedOn = DateTime.Now,
                ModifiedOn = DateTime.Now
            });
    }

    public override void Down()
    {
        Delete.FromTable($"{DbTableNames.CATEGORY_TABLE}")
            .Row(new
            {
                Name = "Unattached",
            });
    }
}

#pragma warning restore 1591 // Missing XML comment for publicly visible type or member