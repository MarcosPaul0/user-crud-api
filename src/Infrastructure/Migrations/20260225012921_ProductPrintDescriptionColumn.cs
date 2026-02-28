using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UserCrud.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ProductPrintDescriptionColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "print_description",
                table: "product",
                type: "character varying(600)",
                maxLength: 600,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "print_description",
                table: "product");
        }
    }
}
