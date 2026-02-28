using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WeddingApi.Migrations
{
    /// <inheritdoc />
    public partial class ChangingEmailAndPhone : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "PhoneNumber",
                table: "WeddingGuests",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "WeddingGuests",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.UpdateData(
                table: "WeddingGuests",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Email", "IsAttending", "PhoneNumber" },
                values: new object[] { null, false, null });

            migrationBuilder.UpdateData(
                table: "WeddingGuests",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Email", "PhoneNumber" },
                values: new object[] { null, null });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "PhoneNumber",
                table: "WeddingGuests",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "WeddingGuests",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "WeddingGuests",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Email", "IsAttending", "PhoneNumber" },
                values: new object[] { "john.doe@example.com", true, "123-456-7890" });

            migrationBuilder.UpdateData(
                table: "WeddingGuests",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Email", "PhoneNumber" },
                values: new object[] { "jane.smith@example.com", "987-654-3210" });
        }
    }
}
