using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnlineRestaurant.Database.Migrations
{
    /// <inheritdoc />
    public partial class ItemNavigationPropertiesFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ItemMenu_Menus_ItemsId1",
                table: "ItemMenu");

            migrationBuilder.DropForeignKey(
                name: "FK_Items_Allergens_AllergenId",
                table: "Items");

            migrationBuilder.DropForeignKey(
                name: "FK_Items_Items_ItemId",
                table: "Items");

            migrationBuilder.DropIndex(
                name: "IX_Items_AllergenId",
                table: "Items");

            migrationBuilder.DropIndex(
                name: "IX_Items_ItemId",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "AllergenId",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "ItemId",
                table: "Items");

            migrationBuilder.RenameColumn(
                name: "ItemsId1",
                table: "ItemMenu",
                newName: "MenusId");

            migrationBuilder.RenameIndex(
                name: "IX_ItemMenu_ItemsId1",
                table: "ItemMenu",
                newName: "IX_ItemMenu_MenusId");

            migrationBuilder.CreateTable(
                name: "AllergenItem",
                columns: table => new
                {
                    AllergensId = table.Column<int>(type: "int", nullable: false),
                    ItemsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AllergenItem", x => new { x.AllergensId, x.ItemsId });
                    table.ForeignKey(
                        name: "FK_AllergenItem_Allergens_AllergensId",
                        column: x => x.AllergensId,
                        principalTable: "Allergens",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AllergenItem_Items_ItemsId",
                        column: x => x.ItemsId,
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AllergenItem_ItemsId",
                table: "AllergenItem",
                column: "ItemsId");

            migrationBuilder.AddForeignKey(
                name: "FK_ItemMenu_Menus_MenusId",
                table: "ItemMenu",
                column: "MenusId",
                principalTable: "Menus",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ItemMenu_Menus_MenusId",
                table: "ItemMenu");

            migrationBuilder.DropTable(
                name: "AllergenItem");

            migrationBuilder.RenameColumn(
                name: "MenusId",
                table: "ItemMenu",
                newName: "ItemsId1");

            migrationBuilder.RenameIndex(
                name: "IX_ItemMenu_MenusId",
                table: "ItemMenu",
                newName: "IX_ItemMenu_ItemsId1");

            migrationBuilder.AddColumn<int>(
                name: "AllergenId",
                table: "Items",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ItemId",
                table: "Items",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Items_AllergenId",
                table: "Items",
                column: "AllergenId");

            migrationBuilder.CreateIndex(
                name: "IX_Items_ItemId",
                table: "Items",
                column: "ItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_ItemMenu_Menus_ItemsId1",
                table: "ItemMenu",
                column: "ItemsId1",
                principalTable: "Menus",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Items_Allergens_AllergenId",
                table: "Items",
                column: "AllergenId",
                principalTable: "Allergens",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Items_Items_ItemId",
                table: "Items",
                column: "ItemId",
                principalTable: "Items",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
