using System;
using Microsoft.EntityFrameworkCore.Migrations;
using NodaTime;

namespace Cen.Wms.Data.Migrations
{
    public partial class TPurchaseTaskLinePalletedStateAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "purchase_task_line_palleted_state",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    expiration_date = table.Column<Instant>(type: "timestamp", nullable: true),
                    expiration_days_plus = table.Column<long>(type: "bigint", nullable: false),
                    qty_normal = table.Column<decimal>(type: "numeric", nullable: false),
                    qty_broken = table.Column<decimal>(type: "numeric", nullable: false),
                    changed_at = table.Column<Instant>(type: "timestamp", nullable: false),
                    purchase_task_line_id = table.Column<Guid>(type: "uuid", nullable: false),
                    xmin = table.Column<uint>(type: "xid", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_purchase_task_line_palleted_state", x => x.id);
                    table.ForeignKey(
                        name: "f_k_purchase_task_line_palleted_state__purchase_task_line_purchase~",
                        column: x => x.purchase_task_line_id,
                        principalTable: "purchase_task_line",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "i_x_purchase_task_line_palleted_state_purchase_task_line_id",
                table: "purchase_task_line_palleted_state",
                column: "purchase_task_line_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "purchase_task_line_palleted_state");
        }
    }
}
