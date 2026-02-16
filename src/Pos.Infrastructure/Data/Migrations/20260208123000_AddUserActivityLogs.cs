using System;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Pos.Infrastructure.Data;

#nullable disable

namespace Pos.Infrastructure.Data.Migrations
{
    [DbContext(typeof(PosDbContext))]
    [Migration("20260208123000_AddUserActivityLogs")]
    public partial class AddUserActivityLogs : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserActivityLogs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    CashBoxId = table.Column<Guid>(type: "uuid", nullable: true),
                    ProductId = table.Column<Guid>(type: "uuid", nullable: true),
                    ActivityType = table.Column<int>(type: "integer", nullable: false),
                    QuantityDelta = table.Column<decimal>(type: "numeric", nullable: true),
                    Amount = table.Column<decimal>(type: "numeric", nullable: true),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserActivityLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserActivityLogs_CashBoxes_CashBoxId",
                        column: x => x.CashBoxId,
                        principalTable: "CashBoxes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_UserActivityLogs_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_UserActivityLogs_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserActivityLogs_CashBoxId",
                table: "UserActivityLogs",
                column: "CashBoxId");

            migrationBuilder.CreateIndex(
                name: "IX_UserActivityLogs_CreatedAt",
                table: "UserActivityLogs",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_UserActivityLogs_ProductId",
                table: "UserActivityLogs",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_UserActivityLogs_UserId",
                table: "UserActivityLogs",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserActivityLogs");
        }
    }
}
