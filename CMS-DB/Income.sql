-- Stored Procedures Incomes
-- Use CommunalManagementSystem

-- Insert income
CREATE PROCEDURE InsertIncome
    @Description NVARCHAR(255),
    @Amount DECIMAL(10, 2),
    @Date DATE
AS
BEGIN
    INSERT INTO incomes (description, amount, date)
    VALUES (@Description, @Amount, @Date);
END;

go

-- Update income
CREATE PROCEDURE UpdateIncome
    @Id UNIQUEIDENTIFIER,
    @Description NVARCHAR(255),
    @Amount DECIMAL(10, 2),
    @Date DATE
AS
BEGIN
    UPDATE incomes
    SET description = @Description,
        amount = @Amount,
        date = @Date
    WHERE id = @Id;
END;

go

-- Delete income
CREATE PROCEDURE DeleteIncome
    @Id UNIQUEIDENTIFIER
AS
BEGIN
    DELETE FROM incomes WHERE id = @Id;
END;

go

-- Get all incomes
CREATE PROCEDURE GetAllIncomes
AS
BEGIN
    SELECT id, description, amount, date, created_at FROM incomes;
END;
