using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlossomServer.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ContactResponse : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ContactResponses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ContactId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ResponseText = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    ResponderId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContactResponses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ContactResponse_Contact_ContactId",
                        column: x => x.ContactId,
                        principalTable: "Contacts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ContactResponse_User_ResponderId",
                        column: x => x.ResponderId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

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

            migrationBuilder.CreateIndex(
                name: "IX_ContactResponses_ContactId",
                table: "ContactResponses",
                column: "ContactId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ContactResponses_ResponderId",
                table: "ContactResponses",
                column: "ResponderId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ContactResponses");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("7e3892c0-9374-49fa-a3fd-53db637a40ae"),
                columns: new[] { "DateOfBirth", "Password" },
                values: new object[] { new DateOnly(2025, 8, 7), "$2a$11$fDB50IB.uahm3VrpzlglsOxoXM/BCRizpYxa/GRT.y7X08GTMKACm" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("98f7750d-655a-4724-812f-50b4ab64012d"),
                columns: new[] { "DateOfBirth", "Password" },
                values: new object[] { new DateOnly(2025, 8, 7), "$2a$11$lK8HL25FbEurcBw2XSPOT.9ruf3Os8AAmr69fqdk8G1IXRIbyn.zq" });
        }
    }
}
