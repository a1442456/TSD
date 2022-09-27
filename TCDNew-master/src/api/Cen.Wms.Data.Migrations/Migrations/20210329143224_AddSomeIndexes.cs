using Microsoft.EntityFrameworkCore.Migrations;

namespace Cen.Wms.Data.Migrations
{
    public partial class AddSomeIndexes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_user_changed_at",
                table: "user",
                column: "changed_at");

            migrationBuilder.CreateIndex(
                name: "IX_user_ext_id",
                table: "user",
                column: "ext_id");

            migrationBuilder.CreateIndex(
                name: "IX_sync_position_name",
                table: "sync_position",
                column: "name");

            migrationBuilder.CreateIndex(
                name: "IX_pac_state_is_busy",
                table: "pac_state",
                column: "is_busy");

            migrationBuilder.CreateIndex(
                name: "IX_pac_state_is_exported",
                table: "pac_state",
                column: "is_exported");

            migrationBuilder.CreateIndex(
                name: "IX_pac_state_is_processed",
                table: "pac_state",
                column: "is_processed");

            migrationBuilder.CreateIndex(
                name: "IX_pac_line_changed_at",
                table: "pac_line",
                column: "changed_at");

            migrationBuilder.CreateIndex(
                name: "IX_pac_line_ext_id",
                table: "pac_line",
                column: "ext_id");

            migrationBuilder.CreateIndex(
                name: "IX_pac_head_changed_at",
                table: "pac_head",
                column: "changed_at");

            migrationBuilder.CreateIndex(
                name: "IX_pac_head_ext_id",
                table: "pac_head",
                column: "ext_id");

            migrationBuilder.CreateIndex(
                name: "IX_facility_changed_at",
                table: "facility",
                column: "changed_at");

            migrationBuilder.CreateIndex(
                name: "IX_facility_ext_id",
                table: "facility",
                column: "ext_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_user_changed_at",
                table: "user");

            migrationBuilder.DropIndex(
                name: "IX_user_ext_id",
                table: "user");

            migrationBuilder.DropIndex(
                name: "IX_sync_position_name",
                table: "sync_position");

            migrationBuilder.DropIndex(
                name: "IX_pac_state_is_busy",
                table: "pac_state");

            migrationBuilder.DropIndex(
                name: "IX_pac_state_is_exported",
                table: "pac_state");

            migrationBuilder.DropIndex(
                name: "IX_pac_state_is_processed",
                table: "pac_state");

            migrationBuilder.DropIndex(
                name: "IX_pac_line_changed_at",
                table: "pac_line");

            migrationBuilder.DropIndex(
                name: "IX_pac_line_ext_id",
                table: "pac_line");

            migrationBuilder.DropIndex(
                name: "IX_pac_head_changed_at",
                table: "pac_head");

            migrationBuilder.DropIndex(
                name: "IX_pac_head_ext_id",
                table: "pac_head");

            migrationBuilder.DropIndex(
                name: "IX_facility_changed_at",
                table: "facility");

            migrationBuilder.DropIndex(
                name: "IX_facility_ext_id",
                table: "facility");
        }
    }
}
