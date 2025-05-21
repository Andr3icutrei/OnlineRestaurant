CREATE PROCEDURE GetItemsForMenu
    @MenuId INT
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT i.*
    FROM Items i
    INNER JOIN MenuItemConfigurations mic ON i.Id = mic.ItemId
    WHERE mic.MenuId = @MenuId
      AND mic.DeletedAt IS NULL
    ORDER BY i.Id;
END