using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlossomServer.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Subcriber : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Subscribers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subscribers", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("7e3892c0-9374-49fa-a3fd-53db637a40ae"),
                columns: new[] { "DateOfBirth", "Password" },
                values: new object[] { new DateOnly(2025, 8, 7), "$2a$11$ik9VYRK0XZDCXvsdS5S0eu/YH9BheUQGTGEFx5DB.r6zLAkMo.2hG" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("98f7750d-655a-4724-812f-50b4ab64012d"),
                columns: new[] { "DateOfBirth", "Password" },
                values: new object[] { new DateOnly(2025, 8, 7), "$2a$11$SOOIvyYibZ0Wasa.X2IJguBbnj93c36xYnyy5Y.IrapDSuMno4bB." });

            migrationBuilder.CreateIndex(
                name: "IX_Subscribers_Email",
                table: "Subscribers",
                column: "Email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Subscribers");

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
        }
    }
}
