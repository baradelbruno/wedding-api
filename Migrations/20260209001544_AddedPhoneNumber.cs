using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WeddingApi.Migrations
{
    /// <inheritdoc />
    public partial class AddedPhoneNumber : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                table: "WeddingGuests",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "WeddingGuests",
                keyColumn: "Id",
                keyValue: 1,
                column: "PhoneNumber",
                value: "123-456-7890");

            migrationBuilder.UpdateData(
                table: "WeddingGuests",
                keyColumn: "Id",
                keyValue: 2,
                column: "PhoneNumber",
                value: "987-654-3210");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "WeddingGuests");
        }
    }
}
