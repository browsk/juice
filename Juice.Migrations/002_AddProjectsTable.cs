using System.Data;
using Migrator.Framework;

namespace Juice.Migrations
{
    [Migration(20081213121000)]
    public class AddProjectsTable : Migration
    {
        public override void Up()
        {
            Database.AddTable("Projects",
                              new Column[]
                                  {
                                      new Column("Id", DbType.Int32, ColumnProperty.PrimaryKeyWithIdentity),
                                      new Column("Name", DbType.String, 128, ColumnProperty.NotNull),
                                      new Column("Description", DbType.String, 255)
                                  });

            Database.AddUniqueConstraint("UC_Projects_Name", "Projects", new string[]{"Name"});
        }

        public override void Down()
        {
            Database.RemoveTable("Projects");
        }
    }
}
