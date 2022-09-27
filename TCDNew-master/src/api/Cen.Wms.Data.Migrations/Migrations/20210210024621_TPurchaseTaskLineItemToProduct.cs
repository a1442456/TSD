using Microsoft.EntityFrameworkCore.Migrations;

namespace Cen.Wms.Data.Migrations
{
    public partial class TPurchaseTaskLineItemToProduct : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "item_name",
                table: "purchase_task_line",
                newName: "product_name");

            migrationBuilder.RenameColumn(
                name: "item_ext_id",
                table: "purchase_task_line",
                newName: "product_ext_id");

            migrationBuilder.RenameColumn(
                name: "item_barcodes",
                table: "purchase_task_line",
                newName: "product_barcodes");

            migrationBuilder.RenameColumn(
                name: "item_abc",
                table: "purchase_task_line",
                newName: "product_abc");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "product_name",
                table: "purchase_task_line",
                newName: "item_name");

            migrationBuilder.RenameColumn(
                name: "product_ext_id",
                table: "purchase_task_line",
                newName: "item_ext_id");

            migrationBuilder.RenameColumn(
                name: "product_barcodes",
                table: "purchase_task_line",
                newName: "item_barcodes");

            migrationBuilder.RenameColumn(
                name: "product_abc",
                table: "purchase_task_line",
                newName: "item_abc");
        }
    }
}
