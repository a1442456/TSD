using System;
using Microsoft.EntityFrameworkCore.Migrations;
using NodaTime;

namespace Cen.Wms.Data.Migrations
{
    public partial class TPacLineStateAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "pac_line_state",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    expiration_date = table.Column<Instant>(type: "timestamp", nullable: true),
                    expiration_days_plus = table.Column<long>(type: "bigint", nullable: false),
                    qty_normal = table.Column<decimal>(type: "numeric", nullable: false),
                    qty_broken = table.Column<decimal>(type: "numeric", nullable: false),
                    changed_at = table.Column<Instant>(type: "timestamp", nullable: false),
                    pac_line_id = table.Column<Guid>(type: "uuid", nullable: false),
                    xmin = table.Column<uint>(type: "xid", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_pac_line_state", x => x.id);
                    table.ForeignKey(
                        name: "f_k_pac_line_state_pac_line_pac_line_id",
                        column: x => x.pac_line_id,
                        principalTable: "pac_line",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "i_x_pac_line_state_pac_line_id",
                table: "pac_line_state",
                column: "pac_line_id",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "pac_line_state");
        }
    }
}
