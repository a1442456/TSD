using Microsoft.EntityFrameworkCore.Migrations;
using NodaTime;

namespace Cen.Wms.Data.Migrations
{
    public partial class TExpirationDateOTypeChanged : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<LocalDate>(
                name: "expiration_date",
                table: "purchase_task_line_update",
                type: "date",
                nullable: true,
                oldClrType: typeof(Instant),
                oldType: "timestamp",
                oldNullable: true);

            migrationBuilder.AlterColumn<LocalDate>(
                name: "expiration_date",
                table: "purchase_task_line_state",
                type: "date",
                nullable: true,
                oldClrType: typeof(Instant),
                oldType: "timestamp",
                oldNullable: true);

            migrationBuilder.AlterColumn<LocalDate>(
                name: "expiration_date",
                table: "purchase_task_line_palleted_state",
                type: "date",
                nullable: true,
                oldClrType: typeof(Instant),
                oldType: "timestamp",
                oldNullable: true);

            migrationBuilder.AlterColumn<LocalDate>(
                name: "expiration_date",
                table: "pac_line_state",
                type: "date",
                nullable: true,
                oldClrType: typeof(Instant),
                oldType: "timestamp",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Instant>(
                name: "expiration_date",
                table: "purchase_task_line_update",
                type: "timestamp",
                nullable: true,
                oldClrType: typeof(LocalDate),
                oldType: "date",
                oldNullable: true);

            migrationBuilder.AlterColumn<Instant>(
                name: "expiration_date",
                table: "purchase_task_line_state",
                type: "timestamp",
                nullable: true,
                oldClrType: typeof(LocalDate),
                oldType: "date",
                oldNullable: true);

            migrationBuilder.AlterColumn<Instant>(
                name: "expiration_date",
                table: "purchase_task_line_palleted_state",
                type: "timestamp",
                nullable: true,
                oldClrType: typeof(LocalDate),
                oldType: "date",
                oldNullable: true);

            migrationBuilder.AlterColumn<Instant>(
                name: "expiration_date",
                table: "pac_line_state",
                type: "timestamp",
                nullable: true,
                oldClrType: typeof(LocalDate),
                oldType: "date",
                oldNullable: true);
        }
    }
}
