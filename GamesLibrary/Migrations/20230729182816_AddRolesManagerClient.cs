using Microsoft.EntityFrameworkCore.Migrations;

namespace GamesLibrary.Migrations
{
    /// <inheritdoc />
    public partial class AddRolesManagerClient : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Insert the "Manager" role
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "Name", "NormalizedName", "ConcurrencyStamp" },
                values: new object[] { "1", "Manager", "MANAGER", "1" });

            // Insert the "Client" role
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "Name", "NormalizedName", "ConcurrencyStamp" },
                values: new object[] { "2", "Client", "CLIENT", "2" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // You can add code here to revert the changes made in the Up method
            // This is useful when you want to roll back the migration
            // For this specific migration, you can remove the rows you inserted in the Up method
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2");
        }
    }
}
