using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api_lib.Migrations
{
    /// <inheritdoc />
    public partial class AddDefaultToCreatedAt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
               name: "CreatedAt",
               table: "Users",
               nullable: false,
               defaultValue: DateTime.UtcNow);

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
