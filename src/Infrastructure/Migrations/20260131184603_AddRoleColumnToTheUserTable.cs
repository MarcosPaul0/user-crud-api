using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AutoriaStore.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddRoleColumnToTheUserTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "role",
                table: "user",
                type: "integer",
                nullable: false,
                defaultValue: 3);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "role",
                table: "user");
        }
    }
}
