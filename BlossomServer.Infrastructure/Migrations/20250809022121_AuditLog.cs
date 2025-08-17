using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlossomServer.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AuditLog : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AuditLogs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TableName = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Operation = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    PrimaryKey = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    ColumnName = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    OldValue = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NewValue = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ChangedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ChangedBy = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    ApplicationUser = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    IpAddress = table.Column<string>(type: "nvarchar(45)", maxLength: 45, nullable: false),
                    UserAgent = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SessionId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    DeletedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditLogs", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("7e3892c0-9374-49fa-a3fd-53db637a40ae"),
                columns: new[] { "DateOfBirth", "Password" },
                values: new object[] { new DateOnly(2025, 8, 9), "$2a$11$Y9MSi8sliaWlLOQUUPo7SOXh997UgRooVyRXYpQyuDG1A6NXqFenC" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("98f7750d-655a-4724-812f-50b4ab64012d"),
                columns: new[] { "DateOfBirth", "Password" },
                values: new object[] { new DateOnly(2025, 8, 9), "$2a$11$JrC83MjCrCISX1pOpE7dO.gLPu8b373ETmLH1kdQI7B6/GV9DMC.." });

            migrationBuilder.CreateIndex(
                name: "IX_AuditLogs_ChangedBy",
                table: "AuditLogs",
                column: "ChangedBy");

            migrationBuilder.CreateIndex(
                name: "IX_AuditLogs_ChangedDate",
                table: "AuditLogs",
                column: "ChangedDate");

            migrationBuilder.CreateIndex(
                name: "IX_AuditLogs_PrimaryKey",
                table: "AuditLogs",
                column: "PrimaryKey");

            migrationBuilder.CreateIndex(
                name: "IX_AuditLogs_TableName",
                table: "AuditLogs",
                column: "TableName");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AuditLogs");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("7e3892c0-9374-49fa-a3fd-53db637a40ae"),
                columns: new[] { "DateOfBirth", "Password" },
                values: new object[] { new DateOnly(2025, 8, 8), "$2a$11$Xn/QnAzZyoXJUnuoUHoOA.mcPu8rrSTTbG1DX7NqJxVeea8uGQ.G6" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("98f7750d-655a-4724-812f-50b4ab64012d"),
                columns: new[] { "DateOfBirth", "Password" },
                values: new object[] { new DateOnly(2025, 8, 8), "$2a$11$sjx72lE1U8mevrdNGk4L4OFb72QxWN109D12V7dgncT9ei2tbhuBK" });
        }
    }
}
