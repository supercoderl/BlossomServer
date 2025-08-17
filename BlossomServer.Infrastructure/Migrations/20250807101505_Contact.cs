using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlossomServer.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Contact : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Contacts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contacts", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("7e3892c0-9374-49fa-a3fd-53db637a40ae"),
                column: "Password",
                value: "$2a$11$fDB50IB.uahm3VrpzlglsOxoXM/BCRizpYxa/GRT.y7X08GTMKACm");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("98f7750d-655a-4724-812f-50b4ab64012d"),
                column: "Password",
                value: "$2a$11$lK8HL25FbEurcBw2XSPOT.9ruf3Os8AAmr69fqdk8G1IXRIbyn.zq");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Contacts");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("7e3892c0-9374-49fa-a3fd-53db637a40ae"),
                column: "Password",
                value: "$2a$11$ik9VYRK0XZDCXvsdS5S0eu/YH9BheUQGTGEFx5DB.r6zLAkMo.2hG");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("98f7750d-655a-4724-812f-50b4ab64012d"),
                column: "Password",
                value: "$2a$11$SOOIvyYibZ0Wasa.X2IJguBbnj93c36xYnyy5Y.IrapDSuMno4bB.");
        }
    }
}
