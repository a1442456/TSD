using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Cen.Wms.Data.Migrations
{
    public partial class TPurchaseTaskFCreatedByUserId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "created_by_user_id",
                table: "purchase_task_head",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "i_x_purchase_task_head_created_by_user_id",
                table: "purchase_task_head",
                column: "created_by_user_id");

            migrationBuilder.AddForeignKey(
                name: "f_k_purchase_task_head__user_created_by_user_id",
                table: "purchase_task_head",
                column: "created_by_user_id",
                principalTable: "user",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "f_k_purchase_task_head__user_created_by_user_id",
                table: "purchase_task_head");

            migrationBuilder.DropIndex(
                name: "i_x_purchase_task_head_created_by_user_id",
                table: "purchase_task_head");

            migrationBuilder.DropColumn(
                name: "created_by_user_id",
                table: "purchase_task_head");
        }
    }
}
