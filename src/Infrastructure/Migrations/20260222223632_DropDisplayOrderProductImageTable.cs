// <copyright file="20260222223632_DropDisplayOrderProductImageTable.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

#nullable disable

namespace AutoriaStore.Infrastructure.Migrations
{
    using Microsoft.EntityFrameworkCore.Migrations;

    /// <inheritdoc />
    public partial class DropDisplayOrderProductImageTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_product_image_product_id_display_order",
                table: "product_image");

            migrationBuilder.CreateIndex(
                name: "IX_product_image_product_id",
                table: "product_image",
                column: "product_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_product_image_product_id",
                table: "product_image");

            migrationBuilder.CreateIndex(
                name: "IX_product_image_product_id_display_order",
                table: "product_image",
                columns: new[] { "product_id", "display_order" },
                unique: true);
        }
    }
}
