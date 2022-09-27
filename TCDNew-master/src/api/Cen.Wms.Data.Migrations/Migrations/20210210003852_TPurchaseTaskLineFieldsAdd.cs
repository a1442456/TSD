using Microsoft.EntityFrameworkCore.Migrations;
using NodaTime;

namespace Cen.Wms.Data.Migrations
{
    public partial class TPurchaseTaskLineFieldsAdd : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Instant>(
                name: "changed_at",
                table: "purchase_task_head",
                type: "timestamp",
                nullable: false,
                defaultValue: NodaTime.Instant.FromUnixTimeTicks(0L));

            migrationBuilder.AddColumn<Instant>(
                name: "created_at",
                table: "purchase_task_head",
                type: "timestamp",
                nullable: false,
                defaultValue: NodaTime.Instant.FromUnixTimeTicks(0L));

            migrationBuilder.AddColumn<string>(
                name: "ext_id",
                table: "purchase_task_head",
                type: "text",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "changed_at",
                table: "purchase_task_head");

            migrationBuilder.DropColumn(
                name: "created_at",
                table: "purchase_task_head");

            migrationBuilder.DropColumn(
                name: "ext_id",
                table: "purchase_task_head");
        }
    }
}
