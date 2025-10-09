-- Create database CommunalManagementSystem
-- Use CommunalManagementSystem
-- =============================================
-- COMMUNAL MANAGEMENT SYSTEM - SQL SERVER VERSION
-- Author: Leonardo
-- Date: 2025-06-08
-- =============================================

-- =============================================
-- Table: persons
-- =============================================
CREATE TABLE persons (
    id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
	dni NVARCHAR(20) NOT NULL UNIQUE,
    name NVARCHAR(100) NOT NULL,
    phone NVARCHAR(20),
    created_at DATETIME2 NOT NULL DEFAULT SYSDATETIME()
);

-- =============================================
-- Table: auth_users (authentication profiles for privileged roles)
-- =============================================
CREATE TABLE auth_users (
    id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    person_id UNIQUEIDENTIFIER NOT NULL UNIQUE,
    email NVARCHAR(100) NOT NULL UNIQUE,
    password NVARCHAR(255) NOT NULL,
    role NVARCHAR(20) NOT NULL CHECK (role IN ('admin', 'reader')),
    created_at DATETIME2 NOT NULL DEFAULT SYSDATETIME(),
    CONSTRAINT FK_AuthUsers_Person FOREIGN KEY (person_id) REFERENCES persons(id) ON DELETE CASCADE
);

-- =============================================
-- Table: quotas
-- =============================================
CREATE TABLE quotas (
    id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    person_id UNIQUEIDENTIFIER NOT NULL,
    year INT NOT NULL,
    month INT NOT NULL,
    amount DECIMAL(10, 2) NOT NULL,
    created_at DATETIME2 NOT NULL DEFAULT SYSDATETIME(),
    CONSTRAINT FK_Quotas_Person FOREIGN KEY (person_id) REFERENCES persons(id) ON DELETE CASCADE,
    CONSTRAINT UQ_Quotas_Person_Year_Month UNIQUE (person_id, year, month)
);

-- =============================================
-- Table: incomes
-- =============================================
CREATE TABLE incomes (
    id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    description NVARCHAR(255) NOT NULL,
    amount DECIMAL(10, 2) NOT NULL,
    date DATE NOT NULL,
    created_at DATETIME2 NOT NULL DEFAULT SYSDATETIME()
);

-- =============================================
-- Table: expenses
-- =============================================
CREATE TABLE expenses (
    id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    description NVARCHAR(255) NOT NULL,
    amount DECIMAL(10, 2) NOT NULL,
    date DATE NOT NULL,
    created_at DATETIME2 NOT NULL DEFAULT SYSDATETIME()
);
