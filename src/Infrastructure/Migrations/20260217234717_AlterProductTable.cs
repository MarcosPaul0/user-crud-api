// <copyright file="20260217234717_AlterProductTable.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

#nullable disable

namespace AutoriaStore.Infrastructure.Migrations
{
    using Microsoft.EntityFrameworkCore.Migrations;

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
