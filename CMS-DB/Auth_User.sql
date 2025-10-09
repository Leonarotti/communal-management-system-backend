-- Stored Procedures auth_users

-- Use CommunalManagementSystem

CREATE PROCEDURE CreateAuthUser
    @person_id UNIQUEIDENTIFIER,
    @email NVARCHAR(100),
    @password NVARCHAR(255), -- hashed temporary password
    @role NVARCHAR(20) -- only 'admin' or 'reader'
AS
BEGIN
    SET NOCOUNT ON;

    -- Validar que la persona existe
    IF NOT EXISTS (SELECT 1 FROM persons WHERE id = @person_id)
    BEGIN
        RAISERROR('Person not found.', 16, 1);
        RETURN;
    END

    -- Validar que no tenga ya un auth_user
    IF EXISTS (SELECT 1 FROM auth_users WHERE person_id = @person_id)
    BEGIN
        RAISERROR('Auth user already exists for this person.', 16, 1);
        RETURN;
    END

    -- Validar rol
    IF @role NOT IN ('admin', 'reader')
    BEGIN
        RAISERROR('Invalid role. Only admin or reader allowed.', 16, 1);
        RETURN;
    END

    -- Insertar nuevo usuario autenticable
    INSERT INTO auth_users (person_id, email, password, role)
    VALUES (@person_id, @email, @password, @role);
END;

go

CREATE PROCEDURE AuthenticateUser
    @Email NVARCHAR(100),
    @Password NVARCHAR(255)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT a.id AS auth_id, a.person_id, p.name, a.role
    FROM auth_users a
    JOIN persons p ON a.person_id = p.id
    WHERE a.email = @Email AND a.password = @Password;
END;

go

CREATE PROCEDURE GetAuthUsers
AS
BEGIN
    SET NOCOUNT ON;

    SELECT a.id, p.name, p.dni, p.phone , a.email, a.role, a.created_at
    FROM auth_users a
    INNER JOIN persons p ON a.person_id = p.id
    ORDER BY a.created_at DESC;
END;

go

CREATE PROCEDURE UpdateAuthUser
    @id UNIQUEIDENTIFIER,
    @email NVARCHAR(100),
    @password NVARCHAR(255),
    @role NVARCHAR(20)
AS
BEGIN
    SET NOCOUNT ON;

    IF @role NOT IN ('admin', 'reader')
    BEGIN
        RAISERROR('Invalid role. Only admin or reader allowed.', 16, 1);
        RETURN;
    END

    UPDATE auth_users
    SET email = @email,
        password = @password,
        role = @role
    WHERE id = @id;
END;

go

CREATE PROCEDURE DeleteAuthUser
    @id UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;

    DELETE FROM auth_users WHERE id = @id;
END;
