CREATE PROCEDURE IsAllergenTypeUnique
    @AllergenType NVARCHAR(255),
    @IsUnique BIT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    
    -- Check if any allergen with the same type exists
    IF EXISTS (SELECT 1 FROM Allergens WHERE Type = @AllergenType)
        SET @IsUnique = 0; -- Not unique (false)
    ELSE
        SET @IsUnique = 1; -- Unique (true)
END