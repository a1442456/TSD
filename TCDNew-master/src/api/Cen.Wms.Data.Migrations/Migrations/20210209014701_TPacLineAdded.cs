using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using NodaTime;

namespace Cen.Wms.Data.Migrations
{
    public partial class TPacLineAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "pac",
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
                    table.PrimaryKey("p_k_pac", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "pac_line",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    line_num = table.Column<int>(type: "integer", nullable: false),
                    pac_line_id = table.Column<string>(type: "text", nullable: true),
                    product_id = table.Column<string>(type: "text", nullable: true),
                    product_name = table.Column<string>(type: "text", nullable: true),
                    product_barcode_main = table.Column<string>(type: "text", nullable: true),
                    product_unit_of_measure = table.Column<string>(type: "text", nullable: true),
                    qty_expected = table.Column<decimal>(type: "numeric", nullable: false),
                    product_barcodes = table.Column<List<string>>(type: "text[]", nullable: true),
                    ext_id = table.Column<string>(type: "text", nullable: true),
                    changed_at = table.Column<Instant>(type: "timestamp", nullable: false),
                    pac_row_id = table.Column<Guid>(type: "uuid", nullable: true),
                    xmin = table.Column<uint>(type: "xid", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_pac_line", x => x.id);
                    table.ForeignKey(
                        name: "f_k_pac_line__pac_pac_row_id",
                        column: x => x.pac_row_id,
                        principalTable: "pac",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "i_x_pac_line_pac_row_id",
                table: "pac_line",
                column: "pac_row_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "pac_line");

            migrationBuilder.DropTable(
                name: "pac");
        }
    }
}
