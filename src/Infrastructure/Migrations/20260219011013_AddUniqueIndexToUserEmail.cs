using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UserCrud.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddUniqueIndexToUserEmail : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_user_email",
                table: "user",
                column: "email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_user_email",
                table: "user");
        }
    }
}
