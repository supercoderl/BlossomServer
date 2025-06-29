using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlossomServer.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateBookingDetailServiceOptionId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Guid>(
                name: "ServiceId",
                table: "BookingDetails",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddColumn<Guid>(
                name: "ServiceOptionId",
                table: "BookingDetails",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("7e3892c0-9374-49fa-a3fd-53db637a40ae"),
                column: "Password",
                value: "$2a$11$AmzGmkw9HDz9QdUEZtAlhOyLcA.wYI7FmnuX5G9e/y1I5JMey/5X6");

            migrationBuilder.CreateIndex(
                name: "IX_BookingDetails_ServiceOptionId",
                table: "BookingDetails",
                column: "ServiceOptionId");

            migrationBuilder.AddForeignKey(
                name: "FK_BookingDetail_ServiceOption_ServiceOptionId",
                table: "BookingDetails",
                column: "ServiceOptionId",
                principalTable: "ServiceOptions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookingDetail_ServiceOption_ServiceOptionId",
                table: "BookingDetails");

            migrationBuilder.DropIndex(
                name: "IX_BookingDetails_ServiceOptionId",
                table: "BookingDetails");

            migrationBuilder.DropColumn(
                name: "ServiceOptionId",
                table: "BookingDetails");

            migrationBuilder.AlterColumn<Guid>(
                name: "ServiceId",
                table: "BookingDetails",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("7e3892c0-9374-49fa-a3fd-53db637a40ae"),
                column: "Password",
                value: "$2a$11$RZ0W3yPN.qezpWtz9oZ.E.or87J2G5pSRzikYOYyJq6craf/IHtUe");
        }
    }
}
