using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Migrator.Framework;
using ForeignKeyConstraint=Migrator.Framework.ForeignKeyConstraint;

namespace Juice.Migrations
{
    [Migration(20081215130300)]
    public class AddTaskEstimateHistoryTable : Migration
    {
        public override void Up()
        {
            Column[] columns = new Column[]
                                   {
                                       new Column("Id", DbType.Int32, ColumnProperty.PrimaryKeyWithIdentity),
                                       new Column("TaskId", DbType.Int32,
                                                  ColumnProperty.ForeignKey | ColumnProperty.NotNull),
                                       new Column("Estimate", DbType.Double, ColumnProperty.NotNull),
                                       new Column("Timestamp", DbType.DateTime, ColumnProperty.NotNull)
                                   };

            Database.AddTable("TaskEstimateHistory", columns);
            Database.AddForeignKey("FK_TaskEstimateHistory_Task", "TaskEstimateHistory", "TaskId", "Tasks", "Id", ForeignKeyConstraint.Cascade);
        }

        public override void Down()
        {
            Database.RemoveTable("TaskEstimateHistory");
        }
    }
}
