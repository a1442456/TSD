using Microsoft.EntityFrameworkCore.Migrations;

namespace Cen.Wms.Data.Migrations
{
    public partial class TPacLineFStateFStates : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "i_x_pac_line_state_pac_line_id",
                table: "pac_line_state");

            migrationBuilder.CreateIndex(
                name: "i_x_pac_line_state_pac_line_id",
                table: "pac_line_state",
                column: "pac_line_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "i_x_pac_line_state_pac_line_id",
                table: "pac_line_state");

            migrationBuilder.CreateIndex(
                name: "i_x_pac_line_state_pac_line_id",
                table: "pac_line_state",
                column: "pac_line_id",
                unique: true);
        }
    }
}
