using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RxMed.Migrations
{
    /// <inheritdoc />
    public partial class Removed_Default_Address_Property : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Addresses_DefaultAddressaddress_id",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_DefaultAddressaddress_id",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "DefaultAddressaddress_id",
                table: "Users");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DefaultAddressaddress_id",
                table: "Users",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_DefaultAddressaddress_id",
                table: "Users",
                column: "DefaultAddressaddress_id");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Addresses_DefaultAddressaddress_id",
                table: "Users",
                column: "DefaultAddressaddress_id",
                principalTable: "Addresses",
                principalColumn: "address_id");
        }
    }
}
