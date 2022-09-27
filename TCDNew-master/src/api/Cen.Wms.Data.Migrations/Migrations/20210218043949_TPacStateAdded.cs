using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Cen.Wms.Data.Migrations
{
    public partial class TPacStateAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "pac_state",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    is_busy = table.Column<bool>(type: "boolean", nullable: false),
                    is_processed = table.Column<bool>(type: "boolean", nullable: false),
                    pac_head_id = table.Column<Guid>(type: "uuid", nullable: false),
                    xmin = table.Column<uint>(type: "xid", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_pac_state", x => x.id);
                    table.ForeignKey(
                        name: "f_k_pac_state_pac_head_pac_head_id",
                        column: x => x.pac_head_id,
                        principalTable: "pac_head",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "i_x_pac_state_pac_head_id",
                table: "pac_state",
                column: "pac_head_id",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "pac_state");
        }
    }
}
