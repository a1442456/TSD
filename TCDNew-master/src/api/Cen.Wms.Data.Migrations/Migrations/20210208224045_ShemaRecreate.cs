using System;
using Microsoft.EntityFrameworkCore.Migrations;
using NodaTime;

namespace Cen.Wms.Data.Migrations
{
    public partial class ShemaRecreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "device",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    device_public_key = table.Column<string>(type: "text", nullable: true),
                    xmin = table.Column<uint>(type: "xid", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_device", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "device_registration_request",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    device_public_key = table.Column<string>(type: "text", nullable: true),
                    created_at = table.Column<Instant>(type: "timestamp", nullable: false),
                    xmin = table.Column<uint>(type: "xid", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_device_registration_request", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "device_registration_request_state",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    is_accepted = table.Column<bool>(type: "boolean", nullable: false),
                    xmin = table.Column<uint>(type: "xid", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_device_registration_request_state", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "device_state",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    device_public_key = table.Column<string>(type: "text", nullable: true),
                    device_status = table.Column<int>(type: "integer", nullable: false),
                    xmin = table.Column<uint>(type: "xid", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_device_state", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "facility",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    ext_id = table.Column<string>(type: "text", nullable: true),
                    changed_at = table.Column<Instant>(type: "timestamp", nullable: false),
                    xmin = table.Column<uint>(type: "xid", rowVersion: true, nullable: false),
                    code = table.Column<string>(type: "character varying(36)", maxLength: 36, nullable: true),
                    name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_facility", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "facility_access",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    facility_id = table.Column<Guid>(type: "uuid", nullable: false),
                    xmin = table.Column<uint>(type: "xid", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_facility_access", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "facility_config",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    acceptance_process_type = table.Column<int>(type: "integer", nullable: false),
                    pallet_code_prefix = table.Column<string>(type: "text", nullable: true),
                    xmin = table.Column<uint>(type: "xid", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_facility_config", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "sync_position",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    position = table.Column<long>(type: "bigint", nullable: false),
                    xmin = table.Column<uint>(type: "xid", rowVersion: true, nullable: false),
                    code = table.Column<string>(type: "character varying(36)", maxLength: 36, nullable: true),
                    name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_sync_position", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "user",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    login = table.Column<string>(type: "text", nullable: true),
                    is_locked_ext = table.Column<bool>(type: "boolean", nullable: false),
                    facility_ext_id = table.Column<string>(type: "text", nullable: true),
                    facility_name = table.Column<string>(type: "text", nullable: true),
                    department_ext_id = table.Column<string>(type: "text", nullable: true),
                    department_name = table.Column<string>(type: "text", nullable: true),
                    position_ext_id = table.Column<string>(type: "text", nullable: true),
                    position_name = table.Column<string>(type: "text", nullable: true),
                    ext_id = table.Column<string>(type: "text", nullable: true),
                    changed_at = table.Column<Instant>(type: "timestamp", nullable: false),
                    xmin = table.Column<uint>(type: "xid", rowVersion: true, nullable: false),
                    code = table.Column<string>(type: "character varying(36)", maxLength: 36, nullable: true),
                    name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_user", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "user_config",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    default_facility_id = table.Column<Guid>(type: "uuid", nullable: false),
                    xmin = table.Column<uint>(type: "xid", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_user_config", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "user_state",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    is_locked = table.Column<bool>(type: "boolean", nullable: false),
                    xmin = table.Column<uint>(type: "xid", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_user_state", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_device_device_public_key",
                table: "device",
                column: "device_public_key",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_device_registration_request_device_public_key",
                table: "device_registration_request",
                column: "device_public_key",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_device_registration_request_device_public_key_created_at",
                table: "device_registration_request",
                columns: new[] { "device_public_key", "created_at" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_device_state_device_public_key",
                table: "device_state",
                column: "device_public_key",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "device");

            migrationBuilder.DropTable(
                name: "device_registration_request");

            migrationBuilder.DropTable(
                name: "device_registration_request_state");

            migrationBuilder.DropTable(
                name: "device_state");

            migrationBuilder.DropTable(
                name: "facility");

            migrationBuilder.DropTable(
                name: "facility_access");

            migrationBuilder.DropTable(
                name: "facility_config");

            migrationBuilder.DropTable(
                name: "sync_position");

            migrationBuilder.DropTable(
                name: "user");

            migrationBuilder.DropTable(
                name: "user_config");

            migrationBuilder.DropTable(
                name: "user_state");
        }
    }
}
