using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Cen.Wms.Data.Migrations
{
    public partial class TPurchaseTaskPacHeadsPlus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "pac_head_id",
                table: "purchase_task_pac_head",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "i_x_purchase_task_pac_head_pac_head_id",
                table: "purchase_task_pac_head",
                column: "pac_head_id");

            migrationBuilder.AddForeignKey(
                name: "f_k_purchase_task_pac_head_pac_head_pac_head_id",
                table: "purchase_task_pac_head",
                column: "pac_head_id",
                principalTable: "pac_head",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "f_k_purchase_task_pac_head_pac_head_pac_head_id",
                table: "purchase_task_pac_head");

            migrationBuilder.DropIndex(
                name: "i_x_purchase_task_pac_head_pac_head_id",
                table: "purchase_task_pac_head");

            migrationBuilder.DropColumn(
                name: "pac_head_id",
                table: "purchase_task_pac_head");
        }
    }
}
