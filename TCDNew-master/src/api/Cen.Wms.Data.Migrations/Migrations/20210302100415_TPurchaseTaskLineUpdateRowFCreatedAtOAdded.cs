using Microsoft.EntityFrameworkCore.Migrations;
using NodaTime;

namespace Cen.Wms.Data.Migrations
{
    public partial class TPurchaseTaskLineUpdateRowFCreatedAtOAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Instant>(
                name: "created_at",
                table: "purchase_task_line_update",
                type: "timestamp",
                nullable: false,
                defaultValue: NodaTime.Instant.FromUnixTimeTicks(0L));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "created_at",
                table: "purchase_task_line_update");
        }
    }
}
