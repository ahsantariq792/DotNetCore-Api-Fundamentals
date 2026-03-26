-- =============================================
-- Script: Create Users table for Login/Signup
-- Database: SuperHerosDB (or your database name)
-- =============================================

-- Use your database name
-- USE SuperHerosDB;
-- GO

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Users')
BEGIN
    CREATE TABLE [dbo].[Users] (
        [Id]           INT             IDENTITY(1,1) NOT NULL,
        [Email]        NVARCHAR(256)   NOT NULL,
        [PasswordHash] NVARCHAR(500)   NOT NULL,
        [FullName]     NVARCHAR(200)   NULL,
        [CreatedAt]    DATETIME2(7)    NOT NULL DEFAULT (GETUTCDATE()),
        CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED ([Id] ASC),
        CONSTRAINT [UQ_Users_Email] UNIQUE ([Email])
    );

    CREATE NONCLUSTERED INDEX [IX_Users_Email] ON [dbo].[Users] ([Email] ASC);

    PRINT 'Users table created successfully.';
END
ELSE
BEGIN
    PRINT 'Users table already exists.';
END
GO
