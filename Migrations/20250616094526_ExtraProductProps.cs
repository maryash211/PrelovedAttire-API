using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GradProject_API.Migrations
{
    /// <inheritdoc />
    public partial class ExtraProductProps : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "Products",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "NationalId",
                table: "Products",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                table: "Products",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "UserName",
                table: "Products",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Address",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "NationalId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "UserName",
                table: "Products");
        }
    }
}
