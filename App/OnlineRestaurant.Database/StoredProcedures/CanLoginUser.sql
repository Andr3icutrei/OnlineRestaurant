CREATE PROCEDURE CanLoginUser
    @Email NVARCHAR(255),
    @Password NVARCHAR(255),
    @UserType INT
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT *
    FROM Users
    WHERE Email = @Email
    AND Password = @Password
    AND Type = @UserType
    AND DeletedAt IS NULL;
END