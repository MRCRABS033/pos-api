using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Pos.Infrastructure.Data;

#nullable disable

namespace Pos.Infrastructure.Data.Migrations
{
    [DbContext(typeof(PosDbContext))]
    [Migration("20260214113000_AddSaleItemsSaleProductIndex")]
    public partial class AddSaleItemsSaleProductIndex : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_SaleItems_SaleId_ProductId",
                table: "SaleItems",
                columns: new[] { "SaleId", "ProductId" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_SaleItems_SaleId_ProductId",
                table: "SaleItems");
        }
    }
}
