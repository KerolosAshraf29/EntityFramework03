using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EntityFramework03.Migrations
{
    /// <inheritdoc />
    public partial class UpdateSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Instructors",
                keyColumn: "Id",
                keyValue: 1,
                column: "Email",
                value: "ahmed@gmail.com");

            migrationBuilder.UpdateData(
                table: "Instructors",
                keyColumn: "Id",
                keyValue: 2,
                column: "Email",
                value: "sarah@gmail.com");

            migrationBuilder.UpdateData(
                table: "Instructors",
                keyColumn: "Id",
                keyValue: 3,
                column: "Email",
                value: "tarek@gmail.com");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Instructors",
                keyColumn: "Id",
                keyValue: 1,
                column: "Email",
                value: "ahmed@platform.com");

            migrationBuilder.UpdateData(
                table: "Instructors",
                keyColumn: "Id",
                keyValue: 2,
                column: "Email",
                value: "sarah@platform.com");

            migrationBuilder.UpdateData(
                table: "Instructors",
                keyColumn: "Id",
                keyValue: 3,
                column: "Email",
                value: "tarek@platform.com");
        }
    }
}
