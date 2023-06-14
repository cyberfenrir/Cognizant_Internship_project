using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RxMed.Migrations
{
    /// <inheritdoc />
    public partial class Updated_Table_relation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Users_default_address_id",
                table: "Users");

            migrationBuilder.CreateIndex(
                name: "IX_Users_default_address_id",
                table: "Users",
                column: "default_address_id");

            migrationBuilder.CreateIndex(
                name: "IX_Addresses_user_id",
                table: "Addresses",
                column: "user_id");

            migrationBuilder.AddForeignKey(
                name: "FK_Addresses_Users_user_id",
                table: "Addresses",
                column: "user_id",
                principalTable: "Users",
                principalColumn: "user_id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Addresses_Users_user_id",
                table: "Addresses");

            migrationBuilder.DropIndex(
                name: "IX_Users_default_address_id",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Addresses_user_id",
                table: "Addresses");

            migrationBuilder.CreateIndex(
                name: "IX_Users_default_address_id",
                table: "Users",
                column: "default_address_id",
                unique: true);
        }
    }
}
