using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api_lib.Migrations
{
    /// <inheritdoc />
    public partial class AddAdminAndTestUsers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
        migrationBuilder.InsertData(
            table: "Users",
            columns: new[] { "Name", "Email", "Password", "CreatedAt", "IsAdmin" },
            values: new object[,]
            {
                { "Admin", "admin@admin.com", "jGl25bVBBBW96Qi9Te4V37Fnqchz/Eu4qB9vKrRIqRg=", DateTime.Now, true }, 
                { "Test", "test@test.com", "n4bQgYhMfWWaL+qgxVrQFaO/TxsrC4Is0V1sFbDwCgg=", DateTime.Now, false } 
            });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM Users WHERE Email IN ('admin@admin.com', 'test@test.com');");
        }
    }
}
