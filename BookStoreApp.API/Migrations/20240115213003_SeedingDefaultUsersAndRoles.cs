using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BookStoreApp.API.Migrations
{
    /// <inheritdoc />
    public partial class SeedingDefaultUsersAndRoles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "FirstName", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { "4d154ce2-c7e8-43eb-9abf-e35319e9d9f6", 0, "ebfa628d-0c56-4968-8c78-8c2d8c929733", "user@bookstore.com", false, "System", "User", false, null, "USER@BOOKSTORE.COM", "USER@BOOKSTORE.COM", "AQAAAAIAAYagAAAAELNorPDCXv+6giQoY9f9GTrpnAzwPwRRm4+jBYwGrn9czfiZIZFttu81eVbj24kqCQ==", null, false, "43a15983-a2c0-40a7-9770-d38bd7df1fb7", false, "user@bookstore.com" },
                    { "6795541e-56df-413c-9806-75bbc76180b4", 0, "6d5840e1-7396-4155-89cb-6e36f65f660f", "admin@bookstore.com", false, "System", "Administrator", false, null, "ADMIN@BOOKSTORE.COM", "ADMIN@BOOKSTORE.COM", "AQAAAAIAAYagAAAAEA07kPiP7pQ0Z06b//DB6fBZiRF7rw7rgYHjnADCrQjGSeY0Ske3olp2AudfRhbueg==", null, false, "326f3002-9486-47e6-b4d1-60ae421621ba", false, "admin@bookstore.com" }
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
        }
    }
}
