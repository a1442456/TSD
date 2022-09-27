using System;
using Microsoft.EntityFrameworkCore.Migrations;
using NodaTime;

namespace Cen.Wms.Data.Migrations
{
    public partial class TPacToTPacHeadRenamed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "f_k_pac_line__pac_pac_id",
                table: "pac_line");

            migrationBuilder.DropTable(
                name: "pac");

            migrationBuilder.DropIndex(
                name: "i_x_pac_line_pac_id",
                table: "pac_line");

            migrationBuilder.AddColumn<Guid>(
                name: "pac_head_id",
                table: "pac_line",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "pac_head",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    pac_date_time = table.Column<Instant>(type: "timestamp", nullable: false),
                    facility_id = table.Column<string>(type: "text", nullable: true),
                    supplier_id = table.Column<string>(type: "text", nullable: true),
                    supplier_name = table.Column<string>(type: "text", nullable: true),
                    purchase_booking_id = table.Column<string>(type: "text", nullable: true),
                    purchase_booking_date = table.Column<LocalDate>(type: "date", nullable: false),
                    purchase_id = table.Column<string>(type: "text", nullable: true),
                    purchase_date = table.Column<LocalDate>(type: "date", nullable: false),
                    ext_id = table.Column<string>(type: "text", nullable: true),
                    changed_at = table.Column<Instant>(type: "timestamp", nullable: false),
                    xmin = table.Column<uint>(type: "xid", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_pac_head", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "i_x_pac_line_pac_head_id",
                table: "pac_line",
                column: "pac_head_id");

            migrationBuilder.AddForeignKey(
                name: "f_k_pac_line_pac_head_pac_head_id",
                table: "pac_line",
                column: "pac_head_id",
                principalTable: "pac_head",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "f_k_pac_line_pac_head_pac_head_id",
                table: "pac_line");

            migrationBuilder.DropTable(
                name: "pac_head");

            migrationBuilder.DropIndex(
                name: "i_x_pac_line_pac_head_id",
                table: "pac_line");

            migrationBuilder.DropColumn(
                name: "pac_head_id",
                table: "pac_line");

            migrationBuilder.CreateTable(
                name: "pac",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    changed_at = table.Column<Instant>(type: "timestamp", nullable: false),
                    ext_id = table.Column<string>(type: "text", nullable: true),
                    facility_id = table.Column<string>(type: "text", nullable: true),
                    pac_date_time = table.Column<Instant>(type: "timestamp", nullable: false),
                    purchase_booking_date = table.Column<LocalDate>(type: "date", nullable: false),
                    purchase_booking_id = table.Column<string>(type: "text", nullable: true),
                    purchase_date = table.Column<LocalDate>(type: "date", nullable: false),
                    purchase_id = table.Column<string>(type: "text", nullable: true),
                    supplier_id = table.Column<string>(type: "text", nullable: true),
                    supplier_name = table.Column<string>(type: "text", nullable: true),
                    xmin = table.Column<uint>(type: "xid", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_pac", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "i_x_pac_line_pac_id",
                table: "pac_line",
                column: "pac_id");

            migrationBuilder.AddForeignKey(
                name: "f_k_pac_line__pac_pac_id",
                table: "pac_line",
                column: "pac_id",
                principalTable: "pac",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
