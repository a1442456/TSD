using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Cen.Wms.Data.Migrations
{
    public partial class TPacToTPacHeadRenamed2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "f_k_pac_line_pac_head_pac_head_id",
                table: "pac_line");

            migrationBuilder.DropColumn(
                name: "pac_id",
                table: "pac_line");

            migrationBuilder.AlterColumn<Guid>(
                name: "pac_head_id",
                table: "pac_line",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "f_k_pac_line_pac_head_pac_head_id",
                table: "pac_line",
                column: "pac_head_id",
                principalTable: "pac_head",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "f_k_pac_line_pac_head_pac_head_id",
                table: "pac_line");

            migrationBuilder.AlterColumn<Guid>(
                name: "pac_head_id",
                table: "pac_line",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddColumn<Guid>(
                name: "pac_id",
                table: "pac_line",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddForeignKey(
                name: "f_k_pac_line_pac_head_pac_head_id",
                table: "pac_line",
                column: "pac_head_id",
                principalTable: "pac_head",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
