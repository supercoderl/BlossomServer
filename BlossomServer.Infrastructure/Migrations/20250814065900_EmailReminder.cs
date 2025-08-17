using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlossomServer.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class EmailReminder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EmailReminders",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EntityId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RecipientEmail = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    RecipientName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    RecipientType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReminderType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Subject = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TargetDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ReminderDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsScheduled = table.Column<bool>(type: "bit", nullable: false),
                    IsSent = table.Column<bool>(type: "bit", nullable: false),
                    HangfireJobId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmailReminders", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("7e3892c0-9374-49fa-a3fd-53db637a40ae"),
                columns: new[] { "DateOfBirth", "Password" },
                values: new object[] { new DateOnly(2025, 8, 14), "$2a$11$xy82k7kyWVcJ3t.jd6JKoeUhNzwkL89VTxZyJwvze2MxZqtQpktOy" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("98f7750d-655a-4724-812f-50b4ab64012d"),
                columns: new[] { "DateOfBirth", "Password" },
                values: new object[] { new DateOnly(2025, 8, 14), "$2a$11$SED92X/4/7FM7jUbAZ2MseQh2kkj1SMxlLc/EtG5S7xnQJfbAKA0y" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmailReminders");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("7e3892c0-9374-49fa-a3fd-53db637a40ae"),
                columns: new[] { "DateOfBirth", "Password" },
                values: new object[] { new DateOnly(2025, 8, 13), "$2a$11$8MLqdMlpBzKi5szn70GCvugJEePDB2tWL8WMw4YxgLlahzIRb1Zk6" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("98f7750d-655a-4724-812f-50b4ab64012d"),
                columns: new[] { "DateOfBirth", "Password" },
                values: new object[] { new DateOnly(2025, 8, 13), "$2a$11$.eC0lJRwi.DFYQJknrQFueRjxSpaJu6wxvG5AHzQSx1pkrEXxhCfy" });
        }
    }
}
