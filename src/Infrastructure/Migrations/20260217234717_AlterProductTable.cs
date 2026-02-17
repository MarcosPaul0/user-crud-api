using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UserCrud.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AlterProductTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ProductionTimeInDays",
                table: "product",
                newName: "production_time_in_minutes");

            migrationBuilder.AddColumn<byte>(
                name: "discount_percentage",
                table: "product",
                type: "smallint",
                nullable: false,
                defaultValue: (byte)0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "discount_percentage",
                table: "product");

            migrationBuilder.RenameColumn(
                name: "production_time_in_minutes",
                table: "product",
                newName: "ProductionTimeInDays");
        }
    }
}
