using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Pos.Infrastructure.Data;

#nullable disable

namespace Pos.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    [DbContext(typeof(PosDbContext))]
    [Migration("20260208120000_AddOwnerBootstrap")]
    public partial class AddOwnerBootstrap : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsOwner",
                table: "Users",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.Sql(
                """
                WITH owner_candidate AS (
                    SELECT "Id"
                    FROM "Users"
                    ORDER BY "Created"
                    LIMIT 1
                )
                UPDATE "Users" u
                SET "IsOwner" = TRUE,
                    "Role" = 'Admin'
                FROM owner_candidate c
                WHERE u."Id" = c."Id"
                  AND NOT EXISTS (
                      SELECT 1 FROM "Users" ux WHERE ux."IsOwner" = TRUE
                  );
                """);

            migrationBuilder.CreateIndex(
                name: "IX_Users_IsOwner",
                table: "Users",
                column: "IsOwner",
                unique: true,
                filter: "\"IsOwner\" = TRUE");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Users_IsOwner",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "IsOwner",
                table: "Users");
        }
    }
}
