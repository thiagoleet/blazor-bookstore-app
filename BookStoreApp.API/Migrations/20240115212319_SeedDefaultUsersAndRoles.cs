using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BookStoreApp.API.Migrations
{
    /// <inheritdoc />
    public partial class SeedDefaultUsersAndRoles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UserName",
                table: "AspNetUsers",
                newName: "Username");

            migrationBuilder.AlterColumn<string>(
                name: "Username",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(256)",
                oldMaxLength: 256,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserName",
                table: "AspNetUsers",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: true);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "5b769c88-641f-40e3-a7dd-b915954d7c45", null, "User", "USER" },
                    { "ea3e0a66-e19b-4e74-9cfe-08ac95b45464", null, "Administrator", "ADMINISTRATOR" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "FirstName", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName", "Username" },
                values: new object[,]
                {
                    { "4d154ce2-c7e8-43eb-9abf-e35319e9d9f6", 0, "efcbac51-8b5b-4720-940d-73cc7175ec6c", "admin@bookstore.com", false, "System", "Administrator", false, null, "ADMIN@BOOKSTORE.COM", "ADMIN@BOOKSTORE.COM", "AQAAAAIAAYagAAAAENV7JMR8D9YZ1StnDeBpoTflK2TY4K4qJ0eLQWoneGfiDoqedDWDZu8uYq9bfc6BGg==", null, false, "54b40067-9eb5-4775-8ae3-dcc8c5a7279e", false, null, "admin@bookstore.com" },
                    { "6795541e-56df-413c-9806-75bbc76180b4", 0, "cb4dbba1-d323-443b-a791-81bfa56e4cbf", "admin@bookstore.com", false, "System", "Administrator", false, null, "ADMIN@BOOKSTORE.COM", "ADMIN@BOOKSTORE.COM", "AQAAAAIAAYagAAAAEH/5g4LaXxRwtICk+HoUfTRmRUX3XfAtRLZ7yu4F0WrPyTMDwurzQR01WKHeNFh+OA==", null, false, "4c2bc8d2-c590-43ee-9a8b-583aa3f8a69e", false, null, "admin@bookstore.com" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[,]
                {
                    { "5b769c88-641f-40e3-a7dd-b915954d7c45", "4d154ce2-c7e8-43eb-9abf-e35319e9d9f6" },
                    { "ea3e0a66-e19b-4e74-9cfe-08ac95b45464", "6795541e-56df-413c-9806-75bbc76180b4" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "5b769c88-641f-40e3-a7dd-b915954d7c45", "4d154ce2-c7e8-43eb-9abf-e35319e9d9f6" });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "ea3e0a66-e19b-4e74-9cfe-08ac95b45464", "6795541e-56df-413c-9806-75bbc76180b4" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "5b769c88-641f-40e3-a7dd-b915954d7c45");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "ea3e0a66-e19b-4e74-9cfe-08ac95b45464");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "4d154ce2-c7e8-43eb-9abf-e35319e9d9f6");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "6795541e-56df-413c-9806-75bbc76180b4");

            migrationBuilder.DropColumn(
                name: "UserName",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "Username",
                table: "AspNetUsers",
                newName: "UserName");

            migrationBuilder.AlterColumn<string>(
                name: "UserName",
                table: "AspNetUsers",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }
    }
}
