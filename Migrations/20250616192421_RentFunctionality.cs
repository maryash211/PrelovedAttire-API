using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GradProject_API.Migrations
{
    /// <inheritdoc />
    public partial class RentFunctionality : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Condition",
                table: "Products",
                newName: "Type");

            migrationBuilder.CreateTable(
                name: "RentalRequests",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhoneNUmber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NationalId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Duration = table.Column<int>(type: "int", nullable: false),
                    RentalStartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RequestDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PickupStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    RenterId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RentalRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RentalRequests_AspNetUsers_RenterId",
                        column: x => x.RenterId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RentalRequests_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_RentalRequests_ProductId",
                table: "RentalRequests",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_RentalRequests_RenterId",
                table: "RentalRequests",
                column: "RenterId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RentalRequests");

            migrationBuilder.RenameColumn(
                name: "Type",
                table: "Products",
                newName: "Condition");
        }
    }
}
