using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Cen.Wms.Data.Migrations
{
    public partial class TPacLineFKTPac : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "f_k_pac_line__pac_pac_row_id",
                table: "pac_line");

            migrationBuilder.DropIndex(
                name: "i_x_pac_line_pac_row_id",
                table: "pac_line");

            migrationBuilder.DropColumn(
                name: "pac_row_id",
                table: "pac_line");

            migrationBuilder.AddColumn<Guid>(
                name: "pac_id",
                table: "pac_line",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "i_x_pac_line_pac_id",
                table: "pac_line",
                column: "pac_id");

            migrationBuilder.AddForeignKey(
                name: "f_k_pac_line__pac_pac_id",
                table: "pac_line",
                column: "pac_id",
                principalTable: "pac",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "f_k_pac_line__pac_pac_id",
                table: "pac_line");

            migrationBuilder.DropIndex(
                name: "i_x_pac_line_pac_id",
                table: "pac_line");

            migrationBuilder.DropColumn(
                name: "pac_id",
                table: "pac_line");

            migrationBuilder.AddColumn<Guid>(
                name: "pac_row_id",
                table: "pac_line",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "i_x_pac_line_pac_row_id",
                table: "pac_line",
                column: "pac_row_id");

            migrationBuilder.AddForeignKey(
                name: "f_k_pac_line__pac_pac_row_id",
                table: "pac_line",
                column: "pac_row_id",
                principalTable: "pac",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
