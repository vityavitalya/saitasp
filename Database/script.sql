IF DB_ID('BookShelfDb') IS NULL
    CREATE DATABASE BookShelfDb;
GO

USE BookShelfDb;
GO

IF OBJECT_ID('Users') IS NULL
CREATE TABLE Users (
    Id           INT IDENTITY(1,1) PRIMARY KEY,
    Name         NVARCHAR(50)  NOT NULL,
    Email        NVARCHAR(100) NOT NULL UNIQUE,
    PasswordHash NVARCHAR(100) NOT NULL,
    Role         NVARCHAR(20)  NOT NULL DEFAULT 'User',
    CreatedAt    DATETIME      NOT NULL DEFAULT GETDATE()
);

IF OBJECT_ID('Items') IS NULL
CREATE TABLE Items (
    Id          INT IDENTITY(1,1) PRIMARY KEY,
    Title       NVARCHAR(200)  NOT NULL,
    Author      NVARCHAR(100)  NOT NULL,
    Year        INT            NOT NULL,
    Description NVARCHAR(2000) NULL,
    UserId      INT            NOT NULL FOREIGN KEY REFERENCES Users(Id),
    CreatedAt   DATETIME       NOT NULL DEFAULT GETDATE()
);

IF OBJECT_ID('Messages') IS NULL
CREATE TABLE Messages (
    Id        INT IDENTITY(1,1) PRIMARY KEY,
    Name      NVARCHAR(50)   NOT NULL,
    Email     NVARCHAR(100)  NOT NULL,
    Text      NVARCHAR(2000) NOT NULL,
    CreatedAt DATETIME       NOT NULL DEFAULT GETDATE()
);
GO
