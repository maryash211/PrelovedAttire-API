using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GradProject_API.Migrations
{
    /// <inheritdoc />
    public partial class SwapFunctionality : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RentalRequests_AspNetUsers_RenterId",
                table: "RentalRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_RentalRequests_Products_ProductId",
                table: "RentalRequests");

            migrationBuilder.CreateTable(
                name: "SwapRequests",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RequesterName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OfferedProductName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Size = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Color = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NationalId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProductImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RequestDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RequestedProductId = table.Column<int>(type: "int", nullable: false),
                    OfferedProductId = table.Column<int>(type: "int", nullable: false),
                    RequesterId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SwapRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SwapRequests_AspNetUsers_RequesterId",
                        column: x => x.RequesterId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SwapRequests_Products_OfferedProductId",
                        column: x => x.OfferedProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SwapRequests_Products_RequestedProductId",
                        column: x => x.RequestedProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SwapRequests_OfferedProductId",
                table: "SwapRequests",
                column: "OfferedProductId");

            migrationBuilder.CreateIndex(
                name: "IX_SwapRequests_RequestedProductId",
                table: "SwapRequests",
                column: "RequestedProductId");

            migrationBuilder.CreateIndex(
                name: "IX_SwapRequests_RequesterId",
                table: "SwapRequests",
                column: "RequesterId");

            migrationBuilder.AddForeignKey(
                name: "FK_RentalRequests_AspNetUsers_RenterId",
                table: "RentalRequests",
                column: "RenterId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RentalRequests_Products_ProductId",
                table: "RentalRequests",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RentalRequests_AspNetUsers_RenterId",
                table: "RentalRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_RentalRequests_Products_ProductId",
                table: "RentalRequests");

            migrationBuilder.DropTable(
                name: "SwapRequests");

            migrationBuilder.AddForeignKey(
                name: "FK_RentalRequests_AspNetUsers_RenterId",
                table: "RentalRequests",
                column: "RenterId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RentalRequests_Products_ProductId",
                table: "RentalRequests",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id");
        }
    }
}
