-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server
-- --------------------------------------------------

--SET QUOTED_IDENTIFIER OFF;
--GO
--USE [bcatpdb2];
--GO
--IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
--GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_MaintenanceHistory_MaintenanceType]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[MaintenanceHistories] DROP CONSTRAINT [FK_MaintenanceHistory_MaintenanceType];
GO
IF OBJECT_ID(N'[dbo].[FK_MaintenanceHistory_CustomerData]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[MaintenanceHistories] DROP CONSTRAINT [FK_MaintenanceHistory_CustomerData];
GO
IF OBJECT_ID(N'[dbo].[FK_CarData_CustomerData]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CarDatas] DROP CONSTRAINT [FK_CarData_CustomerData];
GO
IF OBJECT_ID(N'[dbo].[FK_MaintenanceHistory_CarData]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[MaintenanceHistories] DROP CONSTRAINT [FK_MaintenanceHistory_CarData];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[CustomerDatas]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CustomerDatas];
GO
IF OBJECT_ID(N'[dbo].[MaintenanceHistories]', 'U') IS NOT NULL
    DROP TABLE [dbo].[MaintenanceHistories];
GO
IF OBJECT_ID(N'[dbo].[MaintenanceTypes]', 'U') IS NOT NULL
    DROP TABLE [dbo].[MaintenanceTypes];
GO
IF OBJECT_ID(N'[dbo].[CarDatas]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CarDatas];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

CREATE TABLE [dbo].[CustomerDatas] (
    [CustomerId] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(50) NOT NULL,
    [Address] nvarchar(100) NOT NULL,
    [City] nvarchar(50) NOT NULL,
    [PostalCode] nvarchar(10) NOT NULL,
    [Phone] nvarchar(20) NULL
);
GO

CREATE TABLE [dbo].[MaintenanceHistories] (
    [HistoryId] int IDENTITY(1,1) NOT NULL,
    [CarId] int NOT NULL,
    [MaintId] int NOT NULL,
    [Date] datetime NOT NULL,
    [CustId] int NOT NULL,
    [Cost] decimal(19,4) NULL
);
GO

CREATE TABLE [dbo].[MaintenanceTypes] (
    [MaintId] int IDENTITY(1,1) NOT NULL,
    [TaskName_EN] nvarchar(50) NOT NULL,
    [Cost] nvarchar(50) NOT NULL,
    [Gas] bit NOT NULL,
    [Diesel] bit NOT NULL,
    [Electric] bit NOT NULL,
    [Hybrid] bit NOT NULL,
    [Turbo] bit NOT NULL,
    [TaskName_FR] nvarchar(200) NOT NULL,
    [TaskName_ES] nvarchar(200) NOT NULL
);
GO

CREATE TABLE [dbo].[CarDatas] (
    [CarId] int IDENTITY(1,1) NOT NULL,
    [CustomerId] int NOT NULL,
    [Manufacturer] nvarchar(50) NOT NULL,
    [Model] nvarchar(50) NOT NULL,
    [Year] nvarchar(100) NOT NULL,
    [EngineType] nvarchar(50) NOT NULL,
    [Mileage] nvarchar(50) NULL
);
GO

-- --------------------------------------------------
-- Creating PRIMARY KEY constraints
-- --------------------------------------------------

ALTER TABLE [dbo].[CustomerDatas]
ADD CONSTRAINT [PK_CustomerDatas]
    PRIMARY KEY CLUSTERED ([CustomerId] ASC);
GO

ALTER TABLE [dbo].[MaintenanceHistories]
ADD CONSTRAINT [PK_MaintenanceHistories]
    PRIMARY KEY CLUSTERED ([HistoryId] ASC);
GO

ALTER TABLE [dbo].[MaintenanceTypes]
ADD CONSTRAINT [PK_MaintenanceTypes]
    PRIMARY KEY CLUSTERED ([MaintId] ASC);
GO

ALTER TABLE [dbo].[CarDatas]
ADD CONSTRAINT [PK_CarDatas]
    PRIMARY KEY CLUSTERED ([CarId] ASC);
GO

-- --------------------------------------------------
-- Creating FOREIGN KEY constraints WITH CASCADE DELETE
-- --------------------------------------------------

-- MaintenanceHistories → MaintenanceTypes (NO CASCADE)
ALTER TABLE [dbo].[MaintenanceHistories]
ADD CONSTRAINT [FK_MaintenanceHistory_MaintenanceType]
    FOREIGN KEY ([MaintId])
    REFERENCES [dbo].[MaintenanceTypes] ([MaintId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

CREATE INDEX [IX_FK_MaintenanceHistory_MaintenanceType]
ON [dbo].[MaintenanceHistories] ([MaintId]);
GO

-- MaintenanceHistories → CustomerDatas (CASCADE)
ALTER TABLE [dbo].[MaintenanceHistories]
ADD CONSTRAINT [FK_MaintenanceHistory_CustomerData]
    FOREIGN KEY ([CustId])
    REFERENCES [dbo].[CustomerDatas] ([CustomerId])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

CREATE INDEX [IX_FK_MaintenanceHistory_CustomerData]
ON [dbo].[MaintenanceHistories] ([CustId]);
GO

-- CarDatas → CustomerDatas (CASCADE)
ALTER TABLE [dbo].[CarDatas]
ADD CONSTRAINT [FK_CarData_CustomerData]
    FOREIGN KEY ([CustomerId])
    REFERENCES [dbo].[CustomerDatas] ([CustomerId])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

CREATE INDEX [IX_FK_CarData_CustomerData]
ON [dbo].[CarDatas] ([CustomerId]);
GO

-- MaintenanceHistories → CarDatas (CASCADE)
ALTER TABLE [dbo].[MaintenanceHistories]
ADD CONSTRAINT [FK_MaintenanceHistory_CarData]
    FOREIGN KEY ([CarId])
    REFERENCES [dbo].[CarDatas] ([CarId])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

CREATE INDEX [IX_FK_MaintenanceHistory_CarData]
ON [dbo].[MaintenanceHistories] ([CarId]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------
