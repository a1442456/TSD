using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Cen.Wms.Data.Migrations
{
    public partial class TPurchaseTaskPlus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateSequence(
                name: "purchase_task_code_seq");

            migrationBuilder.CreateTable(
                name: "purchase_task_head",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    xmin = table.Column<uint>(type: "xid", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_purchase_task_head", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "purchase_task_line",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    purchase_task_head_id = table.Column<Guid>(type: "uuid", nullable: false),
                    xmin = table.Column<uint>(type: "xid", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_purchase_task_line", x => x.id);
                    table.ForeignKey(
                        name: "f_k_purchase_task_line_purchase_task_head_purchase_task_head_id",
                        column: x => x.purchase_task_head_id,
                        principalTable: "purchase_task_head",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "purchase_task_pallet",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    purchase_task_head_id = table.Column<Guid>(type: "uuid", nullable: false),
                    xmin = table.Column<uint>(type: "xid", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_purchase_task_pallet", x => x.id);
                    table.ForeignKey(
                        name: "f_k_purchase_task_pallet_purchase_task_head_purchase_task_head_~",
                        column: x => x.purchase_task_head_id,
                        principalTable: "purchase_task_head",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "purchase_task_line_state",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    purchase_task_line_id = table.Column<Guid>(type: "uuid", nullable: false),
                    xmin = table.Column<uint>(type: "xid", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_purchase_task_line_state", x => x.id);
                    table.ForeignKey(
                        name: "f_k_purchase_task_line_state_purchase_task_line_purchase_task_l~",
                        column: x => x.purchase_task_line_id,
                        principalTable: "purchase_task_line",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "i_x_purchase_task_line_purchase_task_head_id",
                table: "purchase_task_line",
                column: "purchase_task_head_id");

            migrationBuilder.CreateIndex(
                name: "i_x_purchase_task_line_state_purchase_task_line_id",
                table: "purchase_task_line_state",
                column: "purchase_task_line_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "i_x_purchase_task_pallet_purchase_task_head_id",
                table: "purchase_task_pallet",
                column: "purchase_task_head_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "purchase_task_line_state");

            migrationBuilder.DropTable(
                name: "purchase_task_pallet");

            migrationBuilder.DropTable(
                name: "purchase_task_line");

            migrationBuilder.DropTable(
                name: "purchase_task_head");

            migrationBuilder.DropSequence(
                name: "purchase_task_code_seq");
        }
    }
}
