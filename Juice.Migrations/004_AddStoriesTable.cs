using System.Data;
using Migrator.Framework;
using ForeignKeyConstraint=Migrator.Framework.ForeignKeyConstraint;

namespace Juice.Migrations
{
    [Migration(20081215123430)]
    public class AddStoriesTable : Migration
    {
        public override void Up()
        {
            Column[] columns = new Column[]
                                   {
                                       new Column("Id", DbType.Int32, ColumnProperty.PrimaryKeyWithIdentity),
                                       new Column("Name", DbType.String, 255, ColumnProperty.NotNull),
                                       new Column("Description", DbType.String),
                                       new Column("BusinessValue", DbType.Int32),
                                       new Column("Priority", DbType.Int32),
                                       new Column("Estimate", DbType.Int32), 
                                       new Column("ProjectId", DbType.Int32, ColumnProperty.ForeignKey | ColumnProperty.NotNull), 
                                   };
            Database.AddTable("Stories", columns);
            Database.AddForeignKey("FK_Stories_Project", "Stories", "ProjectId", "Projects", "Id", ForeignKeyConstraint.Cascade);
        }

        public override void Down()
        {
            Database.RemoveTable("Stories");
        }
    }
}
