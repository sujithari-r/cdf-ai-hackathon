-- Renewable Dashboard SQL Server Schema
-- Run this script against your SQL Server instance before starting the application.

IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = N'RenewableDashboard')
BEGIN
    CREATE DATABASE RenewableDashboard;
END
GO

USE RenewableDashboard;
GO

IF OBJECT_ID(N'dbo.Locations', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Locations (
        Id INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
        Name NVARCHAR(100) NOT NULL,
        Latitude FLOAT NOT NULL,
        Longitude FLOAT NOT NULL,
        ElectricityRate DECIMAL(10,4) NOT NULL,
        SolarScore DECIMAL(4,2) NOT NULL,
        Note NVARCHAR(500) NOT NULL,
        CONSTRAINT UQ_Locations_Name UNIQUE (Name)
    );
END
GO

IF OBJECT_ID(N'dbo.MarketIndicators', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.MarketIndicators (
        Id INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
        [Key] NVARCHAR(100) NOT NULL,
        Value DECIMAL(10,4) NOT NULL,
        Description NVARCHAR(500) NULL,
        CONSTRAINT UQ_MarketIndicators_Key UNIQUE ([Key])
    );
END
GO

IF OBJECT_ID(N'dbo.MarketSnapshots', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.MarketSnapshots (
        Id INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
        ElectricityPrice DECIMAL(10,4) NOT NULL,
        CapacityGrowth DECIMAL(10,4) NOT NULL,
        RenewableShare DECIMAL(10,4) NOT NULL,
        TrendJson NVARCHAR(MAX) NOT NULL,
        FetchedAt DATETIME2 NOT NULL
    );
END
GO

IF NOT EXISTS (SELECT 1 FROM dbo.Locations)
BEGIN
    INSERT INTO dbo.Locations (Name, Latitude, Longitude, ElectricityRate, SolarScore, Note)
    VALUES
        (N'Texas', 31.0, -99.0, 0.14, 8.5, N'Strong wind and solar development potential'),
        (N'California', 36.7, -119.4, 0.22, 9.2, N'High electricity prices and strong solar market'),
        (N'Arizona', 34.2, -111.7, 0.13, 9.5, N'Excellent solar resource availability');
END
GO

IF NOT EXISTS (SELECT 1 FROM dbo.MarketIndicators)
BEGIN
    INSERT INTO dbo.MarketIndicators ([Key], Value, Description)
    VALUES
        (N'CapacityGrowth', 8.5, N'Market growth indicator'),
        (N'RenewableShare', 32, N'Renewable penetration');
END
GO
