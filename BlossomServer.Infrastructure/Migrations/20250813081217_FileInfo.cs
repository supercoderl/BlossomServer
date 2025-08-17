using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlossomServer.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FileInfo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FileInfos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FileId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FileInfos", x => x.Id);
                });

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FileInfos");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("7e3892c0-9374-49fa-a3fd-53db637a40ae"),
                columns: new[] { "DateOfBirth", "Password" },
                values: new object[] { new DateOnly(2025, 8, 11), "$2a$11$bzoUHjIL76eN2UD0q33vg.GOz.1iincAdekNR6GPVtRdmWTQ8ot4q" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("98f7750d-655a-4724-812f-50b4ab64012d"),
                columns: new[] { "DateOfBirth", "Password" },
                values: new object[] { new DateOnly(2025, 8, 11), "$2a$11$0JmJb0mmczpAu9lky9J/wuL44d6AfZxCuCmNcglkJEyKM5.DQkN2W" });
        }
    }
}
