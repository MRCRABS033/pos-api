using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pos.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class NOMBRE_MIGRACION : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "CashBoxId",
                table: "Sales",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<int>(
                name: "PaymentType",
                table: "Sales",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<Guid>(
                name: "CashBoxId",
                table: "CashFlows",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "CashBoxes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    OpeningBalance = table.Column<decimal>(type: "numeric", nullable: false),
                    CashTotal = table.Column<decimal>(type: "numeric", nullable: false),
                    CardTotal = table.Column<decimal>(type: "numeric", nullable: false),
                    ExpectatedBalance = table.Column<decimal>(type: "numeric", nullable: false),
                    ActualBalance = table.Column<decimal>(type: "numeric", nullable: false),
                    TicketsCount = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CashBoxes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CashBoxes_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Sales_CashBoxId",
                table: "Sales",
                column: "CashBoxId");

            migrationBuilder.CreateIndex(
                name: "IX_Categories_Name",
                table: "Categories",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CashFlows_CashBoxId",
                table: "CashFlows",
                column: "CashBoxId");

            migrationBuilder.CreateIndex(
                name: "IX_CashBoxes_UserId",
                table: "CashBoxes",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_CashFlows_CashBoxes_CashBoxId",
                table: "CashFlows",
                column: "CashBoxId",
                principalTable: "CashBoxes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Sales_CashBoxes_CashBoxId",
                table: "Sales",
                column: "CashBoxId",
                principalTable: "CashBoxes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CashFlows_CashBoxes_CashBoxId",
                table: "CashFlows");

            migrationBuilder.DropForeignKey(
                name: "FK_Sales_CashBoxes_CashBoxId",
                table: "Sales");

            migrationBuilder.DropTable(
                name: "CashBoxes");

            migrationBuilder.DropIndex(
                name: "IX_Sales_CashBoxId",
                table: "Sales");

            migrationBuilder.DropIndex(
                name: "IX_Categories_Name",
                table: "Categories");

            migrationBuilder.DropIndex(
                name: "IX_CashFlows_CashBoxId",
                table: "CashFlows");

            migrationBuilder.DropColumn(
                name: "CashBoxId",
                table: "Sales");

            migrationBuilder.DropColumn(
                name: "PaymentType",
                table: "Sales");

            migrationBuilder.DropColumn(
                name: "CashBoxId",
                table: "CashFlows");
        }
    }
}
