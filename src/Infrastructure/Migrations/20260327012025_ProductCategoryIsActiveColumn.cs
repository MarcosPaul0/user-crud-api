using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AutoriaStore.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ProductCategoryIsActiveColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "is_active",
                table: "product_category",
                type: "boolean",
                nullable: false,
                defaultValue: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "is_active",
                table: "product_category");
        }
    }
}
