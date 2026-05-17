using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace LoginApp.API.Migrations
{
    /// <inheritdoc />
    public partial class SeedUsers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "users",
                columns: new[] { "Id", "CreatedAt", "Email", "PasswordHash", "Username" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 5, 16, 0, 0, 0, 0, DateTimeKind.Utc), "admin@gmail.com", "$2y$10$S0elRIMODvRzfEsQvBdhyuqQo30NpiUDo/TK6glOh.JNWfrYDBvsO", "admin" },
                    { 2, new DateTime(2026, 5, 16, 0, 0, 0, 0, DateTimeKind.Utc), "dzaki@gmail.com", "$2y$10$1wMlnT0osstFTk9aCt0RvekXN4GEGECS8Xrlywc4.NsfMS5..hvuG", "dzaki" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "users",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "users",
                keyColumn: "Id",
                keyValue: 2);
        }
    }
}
