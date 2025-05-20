using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnlineRestaurant.Database.Migrations
{
    /// <inheritdoc />
    public partial class ItemConfigurationQuantityFieldAddition2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<float>(
                name: "MenuPortionQuantity",
                table: "MenuItemConfigurations",
                type: "real",
                nullable: false,
                defaultValue: 0f,
                oldClrType: typeof(float),
                oldType: "real",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Quantity",
                table: "MenuItemConfigurations",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "MenuItemConfigurations");

            migrationBuilder.AlterColumn<float>(
                name: "MenuPortionQuantity",
                table: "MenuItemConfigurations",
                type: "real",
                nullable: true,
                oldClrType: typeof(float),
                oldType: "real");
        }
    }
}
