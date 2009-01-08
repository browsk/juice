using System.Data;
using Migrator.Framework;
using ForeignKeyConstraint=Migrator.Framework.ForeignKeyConstraint;

namespace Juice.Migrations
{
    [Migration(20081215125000)]
    public class AddTasksTable :Migration
    {
        public override void Up()
        {
            Column[] columns = new Column[]
                                   {
                                       new Column("Id", DbType.Int32, ColumnProperty.PrimaryKeyWithIdentity),
                                       new Column("Name", DbType.String, 255, ColumnProperty.NotNull),
                                       new Column("Description", DbType.String),
                                       new Column("Priority", DbType.Int32),
                                       new Column("SprintId", DbType.Int32,
                                                  ColumnProperty.ForeignKey | ColumnProperty.NotNull),
                                       new Column("StoryId", DbType.Int32,
                                                  ColumnProperty.ForeignKey | ColumnProperty.Null),
                                   };
            Database.AddTable("Tasks", columns);
            Database.AddForeignKey("FK_Tasks_Sprints", "Tasks", "SprintId", "Sprints", "Id", ForeignKeyConstraint.Cascade);
            Database.AddForeignKey("FK_Tasks_Stories", "Tasks", "StoryId", "Projects", "Id");

        }

        public override void Down()
        {
            Database.RemoveTable("Tasks");
        }
    }
}
