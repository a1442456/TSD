using Microsoft.EntityFrameworkCore.Migrations;

namespace Cen.Wms.Data.Migrations
{
    public partial class TPurchaseTaskLinePalletedStateFFsAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "pallet_abc",
                table: "purchase_task_line_palleted_state",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "pallet_code",
                table: "purchase_task_line_palleted_state",
                type: "text",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "pallet_abc",
                table: "purchase_task_line_palleted_state");

            migrationBuilder.DropColumn(
                name: "pallet_code",
                table: "purchase_task_line_palleted_state");
        }
    }
}
