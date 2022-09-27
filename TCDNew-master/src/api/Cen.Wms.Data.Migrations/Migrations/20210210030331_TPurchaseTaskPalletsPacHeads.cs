using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Cen.Wms.Data.Migrations
{
    public partial class TPurchaseTaskPalletsPacHeads : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "purchase_task_pac_head",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    purchase_task_head_id = table.Column<Guid>(type: "uuid", nullable: false),
                    xmin = table.Column<uint>(type: "xid", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_purchase_task_pac_head", x => x.id);
                    table.ForeignKey(
                        name: "f_k_purchase_task_pac_head_purchase_task_head_purchase_task_hea~",
                        column: x => x.purchase_task_head_id,
                        principalTable: "purchase_task_head",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "i_x_purchase_task_pac_head_purchase_task_head_id",
                table: "purchase_task_pac_head",
                column: "purchase_task_head_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "purchase_task_pac_head");
        }
    }
}
