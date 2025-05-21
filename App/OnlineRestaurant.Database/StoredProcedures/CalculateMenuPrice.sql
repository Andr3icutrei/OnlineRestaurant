CREATE PROCEDURE CalculateMenuPrice
    @MenuId INT,
    @TotalPrice DECIMAL(10,2) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    
    -- Check if menu exists
    IF NOT EXISTS (SELECT 1 FROM Menus WHERE Id = @MenuId)
    BEGIN
        SET @TotalPrice = 0;
        RETURN;
    END
    
    -- Calculate the sum of all item prices for the given menu
    -- only including non-deleted menu item configurations
    SELECT @TotalPrice = ISNULL(SUM(i.Price), 0)
    FROM Items i
    INNER JOIN MenuItemConfigurations mic ON i.Id = mic.ItemId
    WHERE mic.MenuId = @MenuId
      AND mic.DeletedAt IS NULL;
    
    -- If no items were found, ensure we return 0 instead of NULL
    IF @TotalPrice IS NULL
        SET @TotalPrice = 0;
END