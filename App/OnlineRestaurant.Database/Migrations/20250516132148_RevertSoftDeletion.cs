using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnlineRestaurant.Database.Migrations
{
    /// <inheritdoc />
    public partial class RevertSoftDeletion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AllergenItem_Allergens_AllergenId",
                table: "AllergenItem");

            migrationBuilder.DropForeignKey(
                name: "FK_AllergenItem_Items_ItemId",
                table: "AllergenItem");

            migrationBuilder.DropForeignKey(
                name: "FK_ItemOrder_Items_ItemId",
                table: "ItemOrder");

            migrationBuilder.DropForeignKey(
                name: "FK_ItemOrder_Orders_OrderId",
                table: "ItemOrder");

            migrationBuilder.DropForeignKey(
                name: "FK_ItemPictures_Items_ItemId",
                table: "ItemPictures");

            migrationBuilder.DropForeignKey(
                name: "FK_Items_FoodCategories_FoodCategoryId",
                table: "Items");

            migrationBuilder.DropForeignKey(
                name: "FK_MenuItemConfigurations_Items_ItemId",
                table: "MenuItemConfigurations");

            migrationBuilder.DropForeignKey(
                name: "FK_MenuItemConfigurations_Menus_MenuId",
                table: "MenuItemConfigurations");

            migrationBuilder.DropForeignKey(
                name: "FK_MenuOrder_Menus_MenuId",
                table: "MenuOrder");

            migrationBuilder.DropForeignKey(
                name: "FK_MenuOrder_Orders_OrderId",
                table: "MenuOrder");

            migrationBuilder.DropForeignKey(
                name: "FK_Menus_FoodCategories_FoodCategoryId",
                table: "Menus");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Users_UserId",
                table: "Orders");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MenuOrder",
                table: "MenuOrder");

            migrationBuilder.DropIndex(
                name: "IX_MenuOrder_MenuId",
                table: "MenuOrder");

            migrationBuilder.DropIndex(
                name: "IX_MenuOrder_OrderId",
                table: "MenuOrder");

            migrationBuilder.DropIndex(
                name: "IX_MenuItemConfigurations_MenuId_ItemId",
                table: "MenuItemConfigurations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ItemOrder",
                table: "ItemOrder");

            migrationBuilder.DropIndex(
                name: "IX_ItemOrder_ItemId",
                table: "ItemOrder");

            migrationBuilder.DropIndex(
                name: "IX_ItemOrder_OrderId",
                table: "ItemOrder");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AllergenItem",
                table: "AllergenItem");

            migrationBuilder.DropIndex(
                name: "IX_AllergenItem_AllergenId",
                table: "AllergenItem");

            migrationBuilder.DropIndex(
                name: "IX_AllergenItem_ItemId",
                table: "AllergenItem");

            migrationBuilder.DropColumn(
                name: "MenuId",
                table: "MenuOrder");

            migrationBuilder.DropColumn(
                name: "OrderId",
                table: "MenuOrder");

            migrationBuilder.DropColumn(
                name: "ItemId",
                table: "ItemOrder");

            migrationBuilder.DropColumn(
                name: "OrderId",
                table: "ItemOrder");

            migrationBuilder.DropColumn(
                name: "AllergenId",
                table: "AllergenItem");

            migrationBuilder.DropColumn(
                name: "ItemId",
                table: "AllergenItem");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "MenuOrder",
                newName: "OrdersId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "ItemOrder",
                newName: "OrdersId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "AllergenItem",
                newName: "ItemsId");

            migrationBuilder.AlterColumn<int>(
                name: "OrdersId",
                table: "MenuOrder",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<int>(
                name: "MenusId",
                table: "MenuOrder",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "OrdersId",
                table: "ItemOrder",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<int>(
                name: "ItemsId",
                table: "ItemOrder",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "ItemsId",
                table: "AllergenItem",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<int>(
                name: "AllergensId",
                table: "AllergenItem",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_MenuOrder",
                table: "MenuOrder",
                columns: new[] { "MenusId", "OrdersId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_ItemOrder",
                table: "ItemOrder",
                columns: new[] { "ItemsId", "OrdersId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_AllergenItem",
                table: "AllergenItem",
                columns: new[] { "AllergensId", "ItemsId" });

            migrationBuilder.CreateIndex(
                name: "IX_MenuOrder_OrdersId",
                table: "MenuOrder",
                column: "OrdersId");

            migrationBuilder.CreateIndex(
                name: "IX_MenuItemConfigurations_MenuId",
                table: "MenuItemConfigurations",
                column: "MenuId");

            migrationBuilder.CreateIndex(
                name: "IX_ItemOrder_OrdersId",
                table: "ItemOrder",
                column: "OrdersId");

            migrationBuilder.CreateIndex(
                name: "IX_AllergenItem_ItemsId",
                table: "AllergenItem",
                column: "ItemsId");

            migrationBuilder.AddForeignKey(
                name: "FK_AllergenItem_Allergens_AllergensId",
                table: "AllergenItem",
                column: "AllergensId",
                principalTable: "Allergens",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AllergenItem_Items_ItemsId",
                table: "AllergenItem",
                column: "ItemsId",
                principalTable: "Items",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ItemOrder_Items_ItemsId",
                table: "ItemOrder",
                column: "ItemsId",
                principalTable: "Items",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ItemOrder_Orders_OrdersId",
                table: "ItemOrder",
                column: "OrdersId",
                principalTable: "Orders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ItemPictures_Items_ItemId",
                table: "ItemPictures",
                column: "ItemId",
                principalTable: "Items",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Items_FoodCategories_FoodCategoryId",
                table: "Items",
                column: "FoodCategoryId",
                principalTable: "FoodCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MenuItemConfigurations_Items_ItemId",
                table: "MenuItemConfigurations",
                column: "ItemId",
                principalTable: "Items",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MenuItemConfigurations_Menus_MenuId",
                table: "MenuItemConfigurations",
                column: "MenuId",
                principalTable: "Menus",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MenuOrder_Menus_MenusId",
                table: "MenuOrder",
                column: "MenusId",
                principalTable: "Menus",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MenuOrder_Orders_OrdersId",
                table: "MenuOrder",
                column: "OrdersId",
                principalTable: "Orders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Menus_FoodCategories_FoodCategoryId",
                table: "Menus",
                column: "FoodCategoryId",
                principalTable: "FoodCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Users_UserId",
                table: "Orders",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AllergenItem_Allergens_AllergensId",
                table: "AllergenItem");

            migrationBuilder.DropForeignKey(
                name: "FK_AllergenItem_Items_ItemsId",
                table: "AllergenItem");

            migrationBuilder.DropForeignKey(
                name: "FK_ItemOrder_Items_ItemsId",
                table: "ItemOrder");

            migrationBuilder.DropForeignKey(
                name: "FK_ItemOrder_Orders_OrdersId",
                table: "ItemOrder");

            migrationBuilder.DropForeignKey(
                name: "FK_ItemPictures_Items_ItemId",
                table: "ItemPictures");

            migrationBuilder.DropForeignKey(
                name: "FK_Items_FoodCategories_FoodCategoryId",
                table: "Items");

            migrationBuilder.DropForeignKey(
                name: "FK_MenuItemConfigurations_Items_ItemId",
                table: "MenuItemConfigurations");

            migrationBuilder.DropForeignKey(
                name: "FK_MenuItemConfigurations_Menus_MenuId",
                table: "MenuItemConfigurations");

            migrationBuilder.DropForeignKey(
                name: "FK_MenuOrder_Menus_MenusId",
                table: "MenuOrder");

            migrationBuilder.DropForeignKey(
                name: "FK_MenuOrder_Orders_OrdersId",
                table: "MenuOrder");

            migrationBuilder.DropForeignKey(
                name: "FK_Menus_FoodCategories_FoodCategoryId",
                table: "Menus");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Users_UserId",
                table: "Orders");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MenuOrder",
                table: "MenuOrder");

            migrationBuilder.DropIndex(
                name: "IX_MenuOrder_OrdersId",
                table: "MenuOrder");

            migrationBuilder.DropIndex(
                name: "IX_MenuItemConfigurations_MenuId",
                table: "MenuItemConfigurations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ItemOrder",
                table: "ItemOrder");

            migrationBuilder.DropIndex(
                name: "IX_ItemOrder_OrdersId",
                table: "ItemOrder");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AllergenItem",
                table: "AllergenItem");

            migrationBuilder.DropIndex(
                name: "IX_AllergenItem_ItemsId",
                table: "AllergenItem");

            migrationBuilder.DropColumn(
                name: "MenusId",
                table: "MenuOrder");

            migrationBuilder.DropColumn(
                name: "ItemsId",
                table: "ItemOrder");

            migrationBuilder.DropColumn(
                name: "AllergensId",
                table: "AllergenItem");

            migrationBuilder.RenameColumn(
                name: "OrdersId",
                table: "MenuOrder",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "OrdersId",
                table: "ItemOrder",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "ItemsId",
                table: "AllergenItem",
                newName: "Id");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "MenuOrder",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<int>(
                name: "MenuId",
                table: "MenuOrder",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "OrderId",
                table: "MenuOrder",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "ItemOrder",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<int>(
                name: "ItemId",
                table: "ItemOrder",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "OrderId",
                table: "ItemOrder",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "AllergenItem",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<int>(
                name: "AllergenId",
                table: "AllergenItem",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ItemId",
                table: "AllergenItem",
                type: "int",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_MenuOrder",
                table: "MenuOrder",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ItemOrder",
                table: "ItemOrder",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AllergenItem",
                table: "AllergenItem",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_MenuOrder_MenuId",
                table: "MenuOrder",
                column: "MenuId");

            migrationBuilder.CreateIndex(
                name: "IX_MenuOrder_OrderId",
                table: "MenuOrder",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_MenuItemConfigurations_MenuId_ItemId",
                table: "MenuItemConfigurations",
                columns: new[] { "MenuId", "ItemId" },
                unique: true,
                filter: "[MenuId] IS NOT NULL AND [ItemId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_ItemOrder_ItemId",
                table: "ItemOrder",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_ItemOrder_OrderId",
                table: "ItemOrder",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_AllergenItem_AllergenId",
                table: "AllergenItem",
                column: "AllergenId");

            migrationBuilder.CreateIndex(
                name: "IX_AllergenItem_ItemId",
                table: "AllergenItem",
                column: "ItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_AllergenItem_Allergens_AllergenId",
                table: "AllergenItem",
                column: "AllergenId",
                principalTable: "Allergens",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_AllergenItem_Items_ItemId",
                table: "AllergenItem",
                column: "ItemId",
                principalTable: "Items",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_ItemOrder_Items_ItemId",
                table: "ItemOrder",
                column: "ItemId",
                principalTable: "Items",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_ItemOrder_Orders_OrderId",
                table: "ItemOrder",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_ItemPictures_Items_ItemId",
                table: "ItemPictures",
                column: "ItemId",
                principalTable: "Items",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Items_FoodCategories_FoodCategoryId",
                table: "Items",
                column: "FoodCategoryId",
                principalTable: "FoodCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_MenuItemConfigurations_Items_ItemId",
                table: "MenuItemConfigurations",
                column: "ItemId",
                principalTable: "Items",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_MenuItemConfigurations_Menus_MenuId",
                table: "MenuItemConfigurations",
                column: "MenuId",
                principalTable: "Menus",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_MenuOrder_Menus_MenuId",
                table: "MenuOrder",
                column: "MenuId",
                principalTable: "Menus",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_MenuOrder_Orders_OrderId",
                table: "MenuOrder",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Menus_FoodCategories_FoodCategoryId",
                table: "Menus",
                column: "FoodCategoryId",
                principalTable: "FoodCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Users_UserId",
                table: "Orders",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
