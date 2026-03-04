using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WeddingApi.Migrations
{
    /// <inheritdoc />
    public partial class AddPixPaymentCodeToGift : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PixPaymentCode",
                table: "Gifts",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PixPaymentCode",
                table: "Gifts");
        }
    }
}
