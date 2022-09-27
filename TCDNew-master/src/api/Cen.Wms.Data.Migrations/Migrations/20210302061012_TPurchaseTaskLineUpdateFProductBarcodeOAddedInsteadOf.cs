using Microsoft.EntityFrameworkCore.Migrations;

namespace Cen.Wms.Data.Migrations
{
    public partial class TPurchaseTaskLineUpdateFProductBarcodeOAddedInsteadOf : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "item_barcode",
                table: "purchase_task_line_update",
                newName: "product_barcode");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "product_barcode",
                table: "purchase_task_line_update",
                newName: "item_barcode");
        }
    }
}
