using Microsoft.EntityFrameworkCore.Migrations;
using NodaTime;

namespace Cen.Wms.Data.Migrations
{
    public partial class TPurchaseTaskChangedAtAdd : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Instant>(
                name: "changed_at",
                table: "purchase_task_pallet",
                type: "timestamp",
                nullable: false,
                defaultValue: NodaTime.Instant.FromUnixTimeTicks(0L));

            migrationBuilder.AddColumn<Instant>(
                name: "changed_at",
                table: "purchase_task_line_state",
                type: "timestamp",
                nullable: false,
                defaultValue: NodaTime.Instant.FromUnixTimeTicks(0L));

            migrationBuilder.AddColumn<Instant>(
                name: "changed_at",
                table: "purchase_task_line",
                type: "timestamp",
                nullable: false,
                defaultValue: NodaTime.Instant.FromUnixTimeTicks(0L));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "changed_at",
                table: "purchase_task_pallet");

            migrationBuilder.DropColumn(
                name: "changed_at",
                table: "purchase_task_line_state");

            migrationBuilder.DropColumn(
                name: "changed_at",
                table: "purchase_task_line");
        }
    }
}
