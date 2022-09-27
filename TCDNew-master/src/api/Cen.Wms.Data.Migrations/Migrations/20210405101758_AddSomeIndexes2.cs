using Microsoft.EntityFrameworkCore.Migrations;

namespace Cen.Wms.Data.Migrations
{
    public partial class AddSomeIndexes2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_purchase_task_head_code",
                table: "purchase_task_head",
                column: "code");

            migrationBuilder.CreateIndex(
                name: "IX_pac_head_pac_date_time",
                table: "pac_head",
                column: "pac_date_time");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_purchase_task_head_code",
                table: "purchase_task_head");

            migrationBuilder.DropIndex(
                name: "IX_pac_head_pac_date_time",
                table: "pac_head");
        }
    }
}
