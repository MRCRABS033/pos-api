using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pos.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class MakeCashBoxIdNullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CashFlows_CashBoxes_CashBoxId",
                table: "CashFlows");

            migrationBuilder.DropForeignKey(
                name: "FK_Sales_CashBoxes_CashBoxId",
                table: "Sales");

            migrationBuilder.AlterColumn<Guid>(
                name: "CashBoxId",
                table: "Sales",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldDefaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AlterColumn<Guid>(
                name: "CashBoxId",
                table: "CashFlows",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldDefaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddForeignKey(
                name: "FK_CashFlows_CashBoxes_CashBoxId",
                table: "CashFlows",
                column: "CashBoxId",
                principalTable: "CashBoxes",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Sales_CashBoxes_CashBoxId",
                table: "Sales",
                column: "CashBoxId",
                principalTable: "CashBoxes",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
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

            migrationBuilder.AlterColumn<Guid>(
                name: "CashBoxId",
                table: "Sales",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "CashBoxId",
                table: "CashFlows",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

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
    }
}
