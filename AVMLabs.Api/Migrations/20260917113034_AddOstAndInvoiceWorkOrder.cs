using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AVMLabs.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddOstAndInvoiceWorkOrder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsOST",
                table: "Tests",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "WOId",
                table: "Invoices",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Invoices_WOId",
                table: "Invoices",
                column: "WOId");

            migrationBuilder.AddForeignKey(
                name: "FK_Invoices_WorkOrders_WOId",
                table: "Invoices",
                column: "WOId",
                principalTable: "WorkOrders",
                principalColumn: "WOId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Invoices_WorkOrders_WOId",
                table: "Invoices");

            migrationBuilder.DropIndex(
                name: "IX_Invoices_WOId",
                table: "Invoices");

            migrationBuilder.DropColumn(
                name: "IsOST",
                table: "Tests");

            migrationBuilder.DropColumn(
                name: "WOId",
                table: "Invoices");
        }
    }
}
