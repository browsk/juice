using System.Data;
using Migrator.Framework;
using ForeignKeyConstraint=Migrator.Framework.ForeignKeyConstraint;

namespace Juice.Migrations
{
    [Migration(20081213121400)]
    public class AddSprintsTable : Migration
    {
        public override void Up()
        {
            Column[] columns = new Column[]
                                   {
                                       new Column("Id", DbType.Int32, ColumnProperty.PrimaryKeyWithIdentity),
                                       new Column("Name", DbType.String, 128, ColumnProperty.NotNull),
                                       new Column("StartDate", DbType.Date, ColumnProperty.NotNull),
                                       new Column("EndDate", DbType.Date, ColumnProperty.NotNull),
                                       new Column("ProjectId", DbType.Int32, ColumnProperty.ForeignKey | ColumnProperty.NotNull),
                                   };
            Database.AddTable("Sprints", columns);
            Database.AddForeignKey("FK_Sprints_Projects", "Sprints", "ProjectId", "Projects", "Id", ForeignKeyConstraint.Cascade);
        }


        public override void Down()
        {
            Database.RemoveForeignKey("Sprints", "FK_Sprints_Projects");
            Database.RemoveTable("Sprints");
        }
    }
}
