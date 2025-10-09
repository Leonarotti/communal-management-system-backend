-- Stored Procedures quotas

-- Insert quota
CREATE PROCEDURE InsertQuota
    @PersonId UNIQUEIDENTIFIER,
    @Year INT,
    @Month INT,
    @Amount DECIMAL(10, 2),
    @Status NVARCHAR(20) = 'unpaid'
AS
BEGIN
    INSERT INTO quotas (person_id, year, month, amount, status)
    VALUES (@PersonId, @Year, @Month, @Amount, @Status);
END;

-- Update quota
CREATE PROCEDURE UpdateQuota
    @Id UNIQUEIDENTIFIER,
    @Amount DECIMAL(10, 2),
    @Status NVARCHAR(20)
AS
BEGIN
    UPDATE quotas
    SET amount = @Amount,
        status = @Status
    WHERE id = @Id;
END;

-- Delete quota
CREATE PROCEDURE DeleteQuota
    @Id UNIQUEIDENTIFIER
AS
BEGIN
    DELETE FROM quotas WHERE id = @Id;
END;

-- Get all quotas
CREATE PROCEDURE GetAllQuotas
AS
BEGIN
    SELECT id, person_id, year, month, amount, status, created_at FROM quotas;
END;

-- Get quotas by person
CREATE PROCEDURE GetQuotasByPerson
    @PersonId UNIQUEIDENTIFIER
AS
BEGIN
    SELECT id, person_id, year, month, amount, status, created_at FROM quotas WHERE person_id = @PersonId;
END;
