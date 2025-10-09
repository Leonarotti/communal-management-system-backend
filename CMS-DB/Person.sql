-- Stored Procedures Persons
-- Use CommunalManagementSystem

CREATE PROCEDURE CreatePerson
    @dni NVARCHAR(20),
    @name NVARCHAR(100),
    @phone NVARCHAR(20) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO persons (dni, name, phone)
    VALUES (@dni, @name, @phone);
END;

go

CREATE PROCEDURE SearchPersons
    @search NVARCHAR(100) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    SELECT id, dni, name, phone, created_at
    FROM persons
    WHERE @search IS NULL
       OR name LIKE '%' + @search + '%'
       OR dni LIKE '%' + @search + '%';
END;

go

CREATE PROCEDURE UpdatePerson
    @id UNIQUEIDENTIFIER,
    @name NVARCHAR(100),
    @dni NVARCHAR(20),
    @phone NVARCHAR(20)
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE persons
    SET name = @name,
        dni = @dni,
        phone = @phone
    WHERE id = @id;
END;

go

CREATE PROCEDURE DeletePerson
    @id UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;

    DELETE FROM persons WHERE id = @id;
END;
