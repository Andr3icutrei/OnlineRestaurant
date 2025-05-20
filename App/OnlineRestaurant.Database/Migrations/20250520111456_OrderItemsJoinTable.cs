using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnlineRestaurant.Database.Migrations
{
    /// <inheritdoc />
    public partial class OrderItemsJoinTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "MenuItemConfigurations");

            migrationBuilder.AddColumn<int>(
                name: "NoItems",
                table: "ItemOrder",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NoItems",
                table: "ItemOrder");

            migrationBuilder.AddColumn<int>(
                name: "Quantity",
                table: "MenuItemConfigurations",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
