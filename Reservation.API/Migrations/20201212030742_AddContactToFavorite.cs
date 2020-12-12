using Microsoft.EntityFrameworkCore.Migrations;

namespace Reservation.API.Migrations
{
    public partial class AddContactToFavorite : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserFavoriteReservations_Contacts_ContactId",
                table: "UserFavoriteReservations");

            migrationBuilder.AlterColumn<int>(
                name: "ContactId",
                table: "UserFavoriteReservations",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_UserFavoriteReservations_Contacts_ContactId",
                table: "UserFavoriteReservations",
                column: "ContactId",
                principalTable: "Contacts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserFavoriteReservations_Contacts_ContactId",
                table: "UserFavoriteReservations");

            migrationBuilder.AlterColumn<int>(
                name: "ContactId",
                table: "UserFavoriteReservations",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_UserFavoriteReservations_Contacts_ContactId",
                table: "UserFavoriteReservations",
                column: "ContactId",
                principalTable: "Contacts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
