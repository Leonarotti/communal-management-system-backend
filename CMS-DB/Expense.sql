-- Stored Procedures Expenses
-- Use CommunalManagementSystem

-- Insert expense
CREATE PROCEDURE InsertExpense
    @Description NVARCHAR(255),
    @Amount DECIMAL(10, 2),
    @Date DATE
AS
BEGIN
    INSERT INTO expenses (description, amount, date)
    VALUES (@Description, @Amount, @Date);
END;

go

-- Update expense
CREATE PROCEDURE UpdateExpense
    @Id UNIQUEIDENTIFIER,
    @Description NVARCHAR(255),
    @Amount DECIMAL(10, 2),
    @Date DATE
AS
BEGIN
    UPDATE expenses
    SET description = @Description,
        amount = @Amount,
        date = @Date
    WHERE id = @Id;
END;

go

-- Delete expense
CREATE PROCEDURE DeleteExpense
    @Id UNIQUEIDENTIFIER
AS
BEGIN
    DELETE FROM expenses WHERE id = @Id;
END;

go

-- Get all expenses
CREATE PROCEDURE GetAllExpenses
AS
BEGIN
    SELECT id, description, amount, date, created_at FROM expenses;
END;
