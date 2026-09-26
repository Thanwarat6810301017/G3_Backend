-- ==========================================================================================
-- รายวิชา: 310-2203 Cloud-Based Backend Integration System
-- กลุ่มที่ 3 (Group 03) | กำหนดส่ง: 29 กันยายน 2569
-- บทบาท: Member 5 – Database Architecture & Core API Specialist
-- ไฟล์: 01_CreateSchema.sql
-- คำอธิบาย: สคริปต์สร้าง Database และตารางทั้งหมดใน SQL Server รองรับ Part B, Bonus 3, Bonus 4
-- ==========================================================================================

-- 1. สร้าง Database (ถ้ายังไม่มี)
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = N'G3_Backend_DB')
BEGIN
    CREATE DATABASE [G3_Backend_DB];
    PRINT 'Database [G3_Backend_DB] created successfully.';
END
GO

USE [G3_Backend_DB];
GO

-- 2. สร้างตาราง OpenLibraryBooks (Part B - API 1: Open Library API)
IF OBJECT_ID(N'dbo.OpenLibraryBooks', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.OpenLibraryBooks (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        WorkKey NVARCHAR(100) NOT NULL,
        Title NVARCHAR(500) NOT NULL,
        Author NVARCHAR(255) NULL,
        FirstPublishYear INT NULL,
        Isbn NVARCHAR(50) NULL,
        CoverUrl NVARCHAR(1000) NULL,
        RawJson NVARCHAR(MAX) NULL,
        CreatedAt DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME()
    );

    CREATE NONCLUSTERED INDEX IX_OpenLibraryBooks_WorkKey ON dbo.OpenLibraryBooks(WorkKey);
    CREATE NONCLUSTERED INDEX IX_OpenLibraryBooks_Title ON dbo.OpenLibraryBooks(Title);
    CREATE NONCLUSTERED INDEX IX_OpenLibraryBooks_Author ON dbo.OpenLibraryBooks(Author);
    PRINT 'Table [dbo.OpenLibraryBooks] created.';
END
GO

-- 3. สร้างตาราง GoogleBooks (Part B - API 2: Google Books API)
IF OBJECT_ID(N'dbo.GoogleBooks', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.GoogleBooks (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        GoogleId NVARCHAR(100) NOT NULL,
        Title NVARCHAR(500) NOT NULL,
        Authors NVARCHAR(500) NULL,
        Publisher NVARCHAR(255) NULL,
        PublishedDate NVARCHAR(50) NULL,
        Description NVARCHAR(MAX) NULL,
        PageCount INT NULL,
        Categories NVARCHAR(255) NULL,
        ThumbnailUrl NVARCHAR(1000) NULL,
        RawJson NVARCHAR(MAX) NULL,
        CreatedAt DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME()
    );

    CREATE NONCLUSTERED INDEX IX_GoogleBooks_GoogleId ON dbo.GoogleBooks(GoogleId);
    CREATE NONCLUSTERED INDEX IX_GoogleBooks_Title ON dbo.GoogleBooks(Title);
    PRINT 'Table [dbo.GoogleBooks] created.';
END
GO

-- 4. สร้างตาราง Countries (Part B - API 3: REST Countries API)
IF OBJECT_ID(N'dbo.Countries', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Countries (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        CommonName NVARCHAR(255) NOT NULL,
        OfficialName NVARCHAR(500) NULL,
        Cca2 NVARCHAR(10) NULL,
        Cca3 NVARCHAR(10) NULL,
        Capital NVARCHAR(255) NULL,
        Region NVARCHAR(100) NULL,
        Subregion NVARCHAR(100) NULL,
        Population BIGINT NULL,
        Area FLOAT NULL,
        FlagUrl NVARCHAR(1000) NULL,
        RawJson NVARCHAR(MAX) NULL,
        CreatedAt DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME()
    );

    CREATE NONCLUSTERED INDEX IX_Countries_CommonName ON dbo.Countries(CommonName);
    CREATE NONCLUSTERED INDEX IX_Countries_Region ON dbo.Countries(Region);
    PRINT 'Table [dbo.Countries] created.';
END
GO

-- 5. สร้างตาราง CatFacts (Part B - API 4: Cat Facts API)
IF OBJECT_ID(N'dbo.CatFacts', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.CatFacts (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        Fact NVARCHAR(MAX) NOT NULL,
        Length INT NOT NULL,
        CreatedAt DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME()
    );

    CREATE NONCLUSTERED INDEX IX_CatFacts_CreatedAt ON dbo.CatFacts(CreatedAt);
    PRINT 'Table [dbo.CatFacts] created.';
END
GO

-- 6. สร้างตาราง NasaApods (Part B - API 5: NASA APOD API)
IF OBJECT_ID(N'dbo.NasaApods', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.NasaApods (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        Date NVARCHAR(50) NOT NULL,
        Title NVARCHAR(500) NOT NULL,
        Explanation NVARCHAR(MAX) NULL,
        Url NVARCHAR(1000) NULL,
        HdUrl NVARCHAR(1000) NULL,
        MediaType NVARCHAR(50) NULL,
        Copyright NVARCHAR(255) NULL,
        RawJson NVARCHAR(MAX) NULL,
        CreatedAt DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME()
    );

    CREATE NONCLUSTERED INDEX IX_NasaApods_Date ON dbo.NasaApods(Date);
    CREATE NONCLUSTERED INDEX IX_NasaApods_Title ON dbo.NasaApods(Title);
    PRINT 'Table [dbo.NasaApods] created.';
END
GO

-- 7. สร้างตาราง XmlRecords (Bonus 3: รองรับการ Import ข้อมูล XML 1,000 Records จาก Member 2)
IF OBJECT_ID(N'dbo.XmlRecords', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.XmlRecords (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        RecordId NVARCHAR(100) NOT NULL,
        Name NVARCHAR(255) NOT NULL,
        Category NVARCHAR(100) NULL,
        Value DECIMAL(18,2) NOT NULL DEFAULT 0.00,
        Status NVARCHAR(50) NULL,
        Description NVARCHAR(MAX) NULL,
        RecordDate NVARCHAR(50) NULL,
        RawXml NVARCHAR(MAX) NULL,
        ImportedAt DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME()
    );

    CREATE NONCLUSTERED INDEX IX_XmlRecords_RecordId ON dbo.XmlRecords(RecordId);
    CREATE NONCLUSTERED INDEX IX_XmlRecords_Name ON dbo.XmlRecords(Name);
    CREATE NONCLUSTERED INDEX IX_XmlRecords_Category ON dbo.XmlRecords(Category);
    PRINT 'Table [dbo.XmlRecords] created.';
END
GO

-- 8. สร้างตาราง JsonRecords (Bonus 4: รองรับการ Import ข้อมูล JSON 1,000 Records จาก Member 3)
IF OBJECT_ID(N'dbo.JsonRecords', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.JsonRecords (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        RecordId NVARCHAR(100) NOT NULL,
        Title NVARCHAR(255) NOT NULL,
        Category NVARCHAR(100) NULL,
        Amount DECIMAL(18,2) NOT NULL DEFAULT 0.00,
        Status NVARCHAR(50) NULL,
        Description NVARCHAR(MAX) NULL,
        RecordDate NVARCHAR(50) NULL,
        RawJson NVARCHAR(MAX) NULL,
        ImportedAt DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME()
    );

    CREATE NONCLUSTERED INDEX IX_JsonRecords_RecordId ON dbo.JsonRecords(RecordId);
    CREATE NONCLUSTERED INDEX IX_JsonRecords_Title ON dbo.JsonRecords(Title);
    CREATE NONCLUSTERED INDEX IX_JsonRecords_Category ON dbo.JsonRecords(Category);
    PRINT 'Table [dbo.JsonRecords] created.';
END
GO

PRINT 'All tables and indexes for Group 03 created successfully.';
