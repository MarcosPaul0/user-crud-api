using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AutoriaStore.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddProductShippingColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "depth_in_centimeters",
                table: "product",
                type: "integer",
                nullable: false,
                defaultValue: 15);

            migrationBuilder.AddColumn<int>(
                name: "height_in_centimeters",
                table: "product",
                type: "integer",
                nullable: false,
                defaultValue: 15);

            migrationBuilder.AddColumn<int>(
                name: "weight_in_grams",
                table: "product",
                type: "integer",
                nullable: false,
                defaultValue: 415);

            migrationBuilder.AddColumn<int>(
                name: "width_in_centimeters",
                table: "product",
                type: "integer",
                nullable: false,
                defaultValue: 15);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "depth_in_centimeters",
                table: "product");

            migrationBuilder.DropColumn(
                name: "height_in_centimeters",
                table: "product");

            migrationBuilder.DropColumn(
                name: "weight_in_grams",
                table: "product");

            migrationBuilder.DropColumn(
                name: "width_in_centimeters",
                table: "product");
        }
    }
}
