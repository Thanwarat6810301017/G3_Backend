-- ==========================================================================================
-- รายวิชา: 310-2203 Cloud-Based Backend Integration System
-- กลุ่มที่ 3 (Group 03) | กำหนดส่ง: 29 กันยายน 2569
-- บทบาท: Member 5 – Database Architecture & Core API Specialist
-- ไฟล์: 03_StoredProcedures.sql
-- คำอธิบาย: Stored Procedures และ Views สำหรับการสืบค้นข้อมูลและสรุปสถิติสำหรับ Dashboard
-- ==========================================================================================

USE [G3_Backend_DB];
GO

-- 1. View: vw_DashboardMetrics (สรุปจำนวนข้อมูลทั้งหมดทุกตาราง)
IF OBJECT_ID(N'dbo.vw_DashboardMetrics', N'V') IS NOT NULL
    DROP VIEW dbo.vw_DashboardMetrics;
GO

CREATE VIEW dbo.vw_DashboardMetrics AS
SELECT 
    (SELECT COUNT(*) FROM dbo.OpenLibraryBooks) AS TotalOpenLibraryBooks,
    (SELECT COUNT(*) FROM dbo.GoogleBooks) AS TotalGoogleBooks,
    (SELECT COUNT(*) FROM dbo.Countries) AS TotalCountries,
    (SELECT COUNT(*) FROM dbo.CatFacts) AS TotalCatFacts,
    (SELECT COUNT(*) FROM dbo.NasaApods) AS TotalNasaApods,
    (SELECT COUNT(*) FROM dbo.XmlRecords) AS TotalXmlRecords,
    (SELECT COUNT(*) FROM dbo.JsonRecords) AS TotalJsonRecords,
    (
        (SELECT COUNT(*) FROM dbo.OpenLibraryBooks) +
        (SELECT COUNT(*) FROM dbo.GoogleBooks) +
        (SELECT COUNT(*) FROM dbo.Countries) +
        (SELECT COUNT(*) FROM dbo.CatFacts) +
        (SELECT COUNT(*) FROM dbo.NasaApods) +
        (SELECT COUNT(*) FROM dbo.XmlRecords) +
        (SELECT COUNT(*) FROM dbo.JsonRecords)
    ) AS GrandTotalRecords;
GO

-- 2. Stored Procedure: sp_GetDashboardSummary
IF OBJECT_ID(N'dbo.sp_GetDashboardSummary', N'P') IS NOT NULL
    DROP PROCEDURE dbo.sp_GetDashboardSummary;
GO

CREATE PROCEDURE dbo.sp_GetDashboardSummary
AS
BEGIN
    SET NOCOUNT ON;
    SELECT * FROM dbo.vw_DashboardMetrics;
END
GO

-- 3. Stored Procedure: sp_SearchAllBooks (ค้นหาหนังสือจากทั้ง 2 แหล่ง: Open Library และ Google Books)
IF OBJECT_ID(N'dbo.sp_SearchAllBooks', N'P') IS NOT NULL
    DROP PROCEDURE dbo.sp_SearchAllBooks;
GO

CREATE PROCEDURE dbo.sp_SearchAllBooks
    @SearchKeyword NVARCHAR(255)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        'OpenLibrary' AS Source,
        Id,
        WorkKey AS ExternalId,
        Title,
        Author AS Authors,
        CAST(FirstPublishYear AS NVARCHAR(50)) AS PublishedYear,
        CoverUrl AS ImageUrl,
        CreatedAt
    FROM dbo.OpenLibraryBooks
    WHERE Title LIKE '%' + @SearchKeyword + '%' OR Author LIKE '%' + @SearchKeyword + '%'

    UNION ALL

    SELECT 
        'GoogleBooks' AS Source,
        Id,
        GoogleId AS ExternalId,
        Title,
        Authors,
        PublishedDate AS PublishedYear,
        ThumbnailUrl AS ImageUrl,
        CreatedAt
    FROM dbo.GoogleBooks
    WHERE Title LIKE '%' + @SearchKeyword + '%' OR Authors LIKE '%' + @SearchKeyword + '%'
    ORDER BY CreatedAt DESC;
END
GO

-- 4. Stored Procedure: sp_BulkInsertXmlRecords (Helper สำหรับการ Insert ข้อมูล XML ก้อนใหญ่)
IF OBJECT_ID(N'dbo.sp_BulkInsertXmlRecords', N'P') IS NOT NULL
    DROP PROCEDURE dbo.sp_BulkInsertXmlRecords;
GO

CREATE PROCEDURE dbo.sp_BulkInsertXmlRecords
    @XmlData XML
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO dbo.XmlRecords (RecordId, Name, Category, Value, Status, Description, RecordDate, RawXml, ImportedAt)
    SELECT
        T.Item.value('(RecordId/text())[1]', 'NVARCHAR(100)') AS RecordId,
        T.Item.value('(Name/text())[1]', 'NVARCHAR(255)') AS Name,
        T.Item.value('(Category/text())[1]', 'NVARCHAR(100)') AS Category,
        ISNULL(T.Item.value('(Value/text())[1]', 'DECIMAL(18,2)'), 0.00) AS Value,
        T.Item.value('(Status/text())[1]', 'NVARCHAR(50)') AS Status,
        T.Item.value('(Description/text())[1]', 'NVARCHAR(MAX)') AS Description,
        T.Item.value('(Date/text())[1]', 'NVARCHAR(50)') AS RecordDate,
        CONVERT(NVARCHAR(MAX), T.Item.query('.')) AS RawXml,
        SYSUTCDATETIME() AS ImportedAt
    FROM @XmlData.nodes('/records/record') AS T(Item);

    SELECT @@ROWCOUNT AS RowsInserted;
END
GO

PRINT 'Stored procedures and views created successfully.';
