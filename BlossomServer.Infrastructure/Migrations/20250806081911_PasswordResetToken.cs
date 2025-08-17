using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlossomServer.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class PasswordResetToken : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Services",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                collation: "SQL_Latin1_General_CP1_CI_AS",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldCollation: "SQL_Latin1_General_CP1_CI_AS");

            migrationBuilder.AlterColumn<string>(
                name: "VariantName",
                table: "ServiceOptions",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                collation: "SQL_Latin1_General_CP1_CI_AS",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldCollation: "SQL_Latin1_General_CP1_CI_AS");

            migrationBuilder.CreateTable(
                name: "PasswordResetTokens",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Token = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    ExpirationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsUsed = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PasswordResetTokens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PasswordResetToken_User_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("7e3892c0-9374-49fa-a3fd-53db637a40ae"),
                columns: new[] { "DateOfBirth", "Password" },
                values: new object[] { new DateOnly(2025, 8, 6), "$2a$11$O3U6EfsmZ3BfZqQLdDp7O.BGIDaOcKpl2I/dzUKY0fnL9PCR3kcOa" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("98f7750d-655a-4724-812f-50b4ab64012d"),
                columns: new[] { "DateOfBirth", "Password" },
                values: new object[] { new DateOnly(2025, 8, 6), "$2a$11$naXgeFBa2p3r12E/aLQK.u3WjTjf/DdvNmVt.uRKyZOMJ2HgfVzVu" });

            migrationBuilder.CreateIndex(
                name: "IX_PasswordResetTokens_UserId",
                table: "PasswordResetTokens",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PasswordResetTokens");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Services",
                type: "nvarchar(max)",
                nullable: false,
                collation: "SQL_Latin1_General_CP1_CI_AS",
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255,
                oldCollation: "SQL_Latin1_General_CP1_CI_AS");

            migrationBuilder.AlterColumn<string>(
                name: "VariantName",
                table: "ServiceOptions",
                type: "nvarchar(max)",
                nullable: false,
                collation: "SQL_Latin1_General_CP1_CI_AS",
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255,
                oldCollation: "SQL_Latin1_General_CP1_CI_AS");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("7e3892c0-9374-49fa-a3fd-53db637a40ae"),
                columns: new[] { "DateOfBirth", "Password" },
                values: new object[] { new DateOnly(2025, 7, 15), "$2a$11$.scxhThOWo1kAF2wsIhWMucApPSHM7cOm..a4RGuixYiFEb2yvWGG" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("98f7750d-655a-4724-812f-50b4ab64012d"),
                columns: new[] { "DateOfBirth", "Password" },
                values: new object[] { new DateOnly(2025, 7, 15), "$2a$11$2ePtlaXHFzw3XTVwGzPbbOlEF9v2A1V8PfkhmlVWTvfKjgCHWBkcG" });
        }
    }
}
