using System.Data;
using Migrator.Framework;

namespace Juice.Migrations
{
    [Migration(20081213142300)]
    public class AddUsersTable : Migration
    {
        public override void Up()
        {
            Column[] columns =
                {
                    new Column("Id", DbType.Int32, ColumnProperty.PrimaryKeyWithIdentity),
                    new Column("Username", DbType.String, 255, ColumnProperty.NotNull),
                    new Column("ApplicationName", DbType.String, 255, ColumnProperty.NotNull),
                    new Column("Email", DbType.String, 100, ColumnProperty.NotNull),
                    new Column("Password", DbType.String, 255, ColumnProperty.NotNull),
                    new Column("PasswordQuestion", DbType.String, 255),
                    new Column("PasswordAnswer", DbType.String, 255),
                    new Column("LastLogin", DbType.DateTime), 
                    new Column("LastPasswordChange", DbType.DateTime), 
                    new Column("IsOnline", DbType.Boolean)
                };
            Database.AddTable("Users", columns);
            Database.AddUniqueConstraint("UC_Users_Username", "Users", "Username");
        }

        public override void Down()
        {
            Database.RemoveTable("Users");
        }
    }
}
