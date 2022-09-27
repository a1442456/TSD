using Microsoft.EntityFrameworkCore.Migrations;

namespace Cen.Wms.Data.Migrations
{
    public partial class TPurchaseTaskHeadFIsPubliclyAvailableOAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "is_publicly_available",
                table: "purchase_task_head",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_purchase_task_head_is_publicly_available",
                table: "purchase_task_head",
                column: "is_publicly_available");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_purchase_task_head_is_publicly_available",
                table: "purchase_task_head");

            migrationBuilder.DropColumn(
                name: "is_publicly_available",
                table: "purchase_task_head");
        }
    }
}
