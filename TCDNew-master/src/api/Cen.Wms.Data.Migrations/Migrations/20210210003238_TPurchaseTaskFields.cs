using System;
using Microsoft.EntityFrameworkCore.Migrations;
using NodaTime;

namespace Cen.Wms.Data.Migrations
{
    public partial class TPurchaseTaskFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Instant>(
                name: "expiration_date",
                table: "purchase_task_line_state",
                type: "timestamp",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "expiration_days_plus",
                table: "purchase_task_line_state",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<decimal>(
                name: "qty_broken",
                table: "purchase_task_line_state",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "qty_normal",
                table: "purchase_task_line_state",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "item_abc",
                table: "purchase_task_line",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string[]>(
                name: "item_barcodes",
                table: "purchase_task_line",
                type: "text[]",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "item_ext_id",
                table: "purchase_task_line",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "item_name",
                table: "purchase_task_line",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "price",
                table: "purchase_task_line",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "quantity",
                table: "purchase_task_line",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "code",
                table: "purchase_task_head",
                type: "character varying(36)",
                maxLength: 36,
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "facility_id",
                table: "purchase_task_head",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "i_x_purchase_task_head_facility_id",
                table: "purchase_task_head",
                column: "facility_id");

            migrationBuilder.AddForeignKey(
                name: "f_k_purchase_task_head_facility_facility_id",
                table: "purchase_task_head",
                column: "facility_id",
                principalTable: "facility",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "f_k_purchase_task_head_facility_facility_id",
                table: "purchase_task_head");

            migrationBuilder.DropIndex(
                name: "i_x_purchase_task_head_facility_id",
                table: "purchase_task_head");

            migrationBuilder.DropColumn(
                name: "expiration_date",
                table: "purchase_task_line_state");

            migrationBuilder.DropColumn(
                name: "expiration_days_plus",
                table: "purchase_task_line_state");

            migrationBuilder.DropColumn(
                name: "qty_broken",
                table: "purchase_task_line_state");

            migrationBuilder.DropColumn(
                name: "qty_normal",
                table: "purchase_task_line_state");

            migrationBuilder.DropColumn(
                name: "item_abc",
                table: "purchase_task_line");

            migrationBuilder.DropColumn(
                name: "item_barcodes",
                table: "purchase_task_line");

            migrationBuilder.DropColumn(
                name: "item_ext_id",
                table: "purchase_task_line");

            migrationBuilder.DropColumn(
                name: "item_name",
                table: "purchase_task_line");

            migrationBuilder.DropColumn(
                name: "price",
                table: "purchase_task_line");

            migrationBuilder.DropColumn(
                name: "quantity",
                table: "purchase_task_line");

            migrationBuilder.DropColumn(
                name: "code",
                table: "purchase_task_head");

            migrationBuilder.DropColumn(
                name: "facility_id",
                table: "purchase_task_head");
        }
    }
}
