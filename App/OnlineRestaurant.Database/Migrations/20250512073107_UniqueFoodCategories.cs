using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnlineRestaurant.Database.Migrations
{
    /// <inheritdoc />
    public partial class UniqueFoodCategories : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Type",
                table: "FoodCategories",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_FoodCategories_Type",
                table: "FoodCategories",
                column: "Type",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_FoodCategories_Type",
                table: "FoodCategories");

            migrationBuilder.AlterColumn<string>(
                name: "Type",
                table: "FoodCategories",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");
        }
    }
}
