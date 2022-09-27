using System;
using Microsoft.EntityFrameworkCore.Migrations;
using NodaTime;

namespace Cen.Wms.Data.Migrations
{
    public partial class TPurchaseTaskLineUpdateRowAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "purchase_task_line_update",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    purchase_task_head_id = table.Column<Guid>(type: "uuid", nullable: false),
                    purchase_task_line_id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    item_barcode = table.Column<string>(type: "text", nullable: true),
                    current_pallet_code = table.Column<string>(type: "text", nullable: true),
                    purchase_task_line_update_type = table.Column<int>(type: "integer", nullable: false),
                    expiration_date = table.Column<Instant>(type: "timestamp", nullable: true),
                    expiration_days_plus = table.Column<int>(type: "integer", nullable: false),
                    qty_normal = table.Column<decimal>(type: "numeric", nullable: false),
                    qty_broken = table.Column<decimal>(type: "numeric", nullable: false),
                    xmin = table.Column<uint>(type: "xid", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_purchase_task_line_update", x => x.id);
                    table.ForeignKey(
                        name: "f_k_purchase_task_line_update__user_user_id",
                        column: x => x.user_id,
                        principalTable: "user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "f_k_purchase_task_line_update_purchase_task_head_purchase_task_~",
                        column: x => x.purchase_task_head_id,
                        principalTable: "purchase_task_head",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "f_k_purchase_task_line_update_purchase_task_line_purchase_task_~",
                        column: x => x.purchase_task_line_id,
                        principalTable: "purchase_task_line",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "i_x_purchase_task_line_update_purchase_task_head_id",
                table: "purchase_task_line_update",
                column: "purchase_task_head_id");

            migrationBuilder.CreateIndex(
                name: "i_x_purchase_task_line_update_purchase_task_line_id",
                table: "purchase_task_line_update",
                column: "purchase_task_line_id");

            migrationBuilder.CreateIndex(
                name: "i_x_purchase_task_line_update_user_id",
                table: "purchase_task_line_update",
                column: "user_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "purchase_task_line_update");
        }
    }
}
