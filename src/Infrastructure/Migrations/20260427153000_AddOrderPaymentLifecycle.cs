// <copyright file="20260427153000_AddOrderPaymentLifecycle.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

#nullable disable

namespace AutoriaStore.Infrastructure.Migrations
{
    using System;
    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class AddOrderPaymentLifecycle : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "cancelled_at",
                table: "order",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "disputed_at",
                table: "order",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "paid_at",
                table: "order",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "payment_expires_at",
                table: "order",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "payment_external_id",
                table: "order",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "payment_id",
                table: "order",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "payment_method",
                table: "order",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: string.Empty);

            migrationBuilder.AddColumn<string>(
                name: "payment_provider",
                table: "order",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: string.Empty);

            migrationBuilder.AddColumn<string>(
                name: "payment_status",
                table: "order",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "Pending");

            migrationBuilder.AddColumn<string>(
                name: "pix_copy_paste_code",
                table: "order",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "pix_qr_code_base64",
                table: "order",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "receipt_url",
                table: "order",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "refunded_at",
                table: "order",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "status",
                table: "order",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "AwaitingPayment");

            migrationBuilder.AddColumn<string>(
                name: "product_name",
                table: "order_product",
                type: "character varying(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: string.Empty);

            migrationBuilder.AddColumn<int>(
                name: "total_price_in_cents",
                table: "order_product",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_order_payment_id",
                table: "order",
                column: "payment_id",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_order_payment_id",
                table: "order");

            migrationBuilder.DropColumn(
                name: "cancelled_at",
                table: "order");

            migrationBuilder.DropColumn(
                name: "disputed_at",
                table: "order");

            migrationBuilder.DropColumn(
                name: "paid_at",
                table: "order");

            migrationBuilder.DropColumn(
                name: "payment_expires_at",
                table: "order");

            migrationBuilder.DropColumn(
                name: "payment_external_id",
                table: "order");

            migrationBuilder.DropColumn(
                name: "payment_id",
                table: "order");

            migrationBuilder.DropColumn(
                name: "payment_method",
                table: "order");

            migrationBuilder.DropColumn(
                name: "payment_provider",
                table: "order");

            migrationBuilder.DropColumn(
                name: "payment_status",
                table: "order");

            migrationBuilder.DropColumn(
                name: "pix_copy_paste_code",
                table: "order");

            migrationBuilder.DropColumn(
                name: "pix_qr_code_base64",
                table: "order");

            migrationBuilder.DropColumn(
                name: "receipt_url",
                table: "order");

            migrationBuilder.DropColumn(
                name: "refunded_at",
                table: "order");

            migrationBuilder.DropColumn(
                name: "status",
                table: "order");

            migrationBuilder.DropColumn(
                name: "product_name",
                table: "order_product");

            migrationBuilder.DropColumn(
                name: "total_price_in_cents",
                table: "order_product");
        }
    }
}
