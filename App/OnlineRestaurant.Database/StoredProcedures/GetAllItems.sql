CREATE PROCEDURE GetAllItemsByOrderId
    @OrderId INT
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT i.*
    FROM Items i
    INNER JOIN ItemOrder io ON i.Id = io.ItemId
    WHERE io.OrdersId = @OrderId
    ORDER BY i.Id;
END