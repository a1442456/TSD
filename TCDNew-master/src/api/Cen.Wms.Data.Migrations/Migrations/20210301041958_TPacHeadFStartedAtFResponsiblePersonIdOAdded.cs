using System;
using Microsoft.EntityFrameworkCore.Migrations;
using NodaTime;

namespace Cen.Wms.Data.Migrations
{
    public partial class TPacHeadFStartedAtFResponsiblePersonIdOAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "responsible_user_id",
                table: "pac_head",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Instant>(
                name: "started_at",
                table: "pac_head",
                type: "timestamp",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "i_x_pac_head_responsible_user_id",
                table: "pac_head",
                column: "responsible_user_id");

            migrationBuilder.AddForeignKey(
                name: "f_k_pac_head__user_responsible_user_id",
                table: "pac_head",
                column: "responsible_user_id",
                principalTable: "user",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "f_k_pac_head__user_responsible_user_id",
                table: "pac_head");

            migrationBuilder.DropIndex(
                name: "i_x_pac_head_responsible_user_id",
                table: "pac_head");

            migrationBuilder.DropColumn(
                name: "responsible_user_id",
                table: "pac_head");

            migrationBuilder.DropColumn(
                name: "started_at",
                table: "pac_head");
        }
    }
}
