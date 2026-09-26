-- ==========================================================================================
-- รายวิชา: 310-2203 Cloud-Based Backend Integration System
-- กลุ่มที่ 3 (Group 03) | กำหนดส่ง: 29 กันยายน 2569
-- บทบาท: Member 5 – Database Architecture & Core API Specialist
-- ไฟล์: 04_VerifyData.sql
-- คำอธิบาย: คิวรี่สำหรับทดสอบและตรวจนับความถูกต้องของข้อมูลทั้งหมดในระบบ
-- ==========================================================================================

USE [G3_Backend_DB];
GO

PRINT '=== 1. Summary of Records Count Across All Tables ===';
EXEC dbo.sp_GetDashboardSummary;
GO

PRINT '=== 2. Sample Open Library Books (Top 3) ===';
SELECT TOP 3 Id, WorkKey, Title, Author, FirstPublishYear, CreatedAt FROM dbo.OpenLibraryBooks;
GO

PRINT '=== 3. Sample Google Books (Top 3) ===';
SELECT TOP 3 Id, GoogleId, Title, Authors, Categories, CreatedAt FROM dbo.GoogleBooks;
GO

PRINT '=== 4. Sample Countries (Top 3) ===';
SELECT TOP 3 Id, CommonName, Capital, Region, Population, CreatedAt FROM dbo.Countries;
GO

PRINT '=== 5. Sample Cat Facts (Top 3) ===';
SELECT TOP 3 Id, Fact, Length, CreatedAt FROM dbo.CatFacts;
GO

PRINT '=== 6. Sample NASA APODs (Top 3) ===';
SELECT TOP 3 Id, Date, Title, MediaType, CreatedAt FROM dbo.NasaApods;
GO

PRINT '=== 7. Sample XML Records (Top 3) ===';
SELECT TOP 3 Id, RecordId, Name, Category, Value, Status, ImportedAt FROM dbo.XmlRecords;
GO

PRINT '=== 8. Sample JSON Records (Top 3) ===';
SELECT TOP 3 Id, RecordId, Title, Category, Amount, Status, ImportedAt FROM dbo.JsonRecords;
GO
