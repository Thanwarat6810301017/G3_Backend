-- ==========================================================================================
-- รายวิชา: 310-2203 Cloud-Based Backend Integration System
-- กลุ่มที่ 3 (Group 03) | กำหนดส่ง: 29 กันยายน 2569
-- บทบาท: Member 5 – Database Architecture & Core API Specialist
-- ไฟล์: 02_SeedData.sql
-- คำอธิบาย: ข้อมูลตัวอย่างเริ่มต้น (Seed Data) สำหรับทั้ง 5 APIs และ XML/JSON Import
-- ==========================================================================================

USE [G3_Backend_DB];
GO

-- 1. Seed OpenLibraryBooks
IF NOT EXISTS (SELECT 1 FROM dbo.OpenLibraryBooks)
BEGIN
    INSERT INTO dbo.OpenLibraryBooks (WorkKey, Title, Author, FirstPublishYear, Isbn, CoverUrl, CreatedAt)
    VALUES 
    (N'/works/OL45804W', N'Clean Code: A Handbook of Agile Software Craftsmanship', N'Robert C. Martin', 2008, N'9780132350884', N'https://covers.openlibrary.org/b/id/8231991-M.jpg', SYSUTCDATETIME()),
    (N'/works/OL27479W', N'The Pragmatic Programmer', N'Andrew Hunt, David Thomas', 1999, N'9780201616224', N'https://covers.openlibrary.org/b/id/8091016-M.jpg', SYSUTCDATETIME()),
    (N'/works/OL17352354W', N'Designing Data-Intensive Applications', N'Martin Kleppmann', 2017, N'9781449373320', N'https://covers.openlibrary.org/b/id/8254823-M.jpg', SYSUTCDATETIME());
    PRINT 'OpenLibraryBooks seeded.';
END
GO

-- 2. Seed GoogleBooks
IF NOT EXISTS (SELECT 1 FROM dbo.GoogleBooks)
BEGIN
    INSERT INTO dbo.GoogleBooks (GoogleId, Title, Authors, Publisher, PublishedDate, Description, PageCount, Categories, ThumbnailUrl, CreatedAt)
    VALUES 
    (N'zyTCAlFPjgYC', N'Design Patterns: Elements of Reusable Object-Oriented Software', N'Erich Gamma, Richard Helm, Ralph Johnson, John Vlissides', N'Addison-Wesley Professional', N'1994-10-31', N'Capturing a wealth of experience about the design of object-oriented software, four top-notch designers present a catalog of simple and succinct solutions to commonly occurring design problems.', 395, N'Computers', N'http://books.google.com/books/content?id=zyTCAlFPjgYC&printsec=frontcover&img=1&zoom=1', SYSUTCDATETIME()),
    (N'7nO4AAAAQBAJ', N'Microservices Patterns', N'Chris Richardson', N'Manning Publications', N'2018-10-27', N'A comprehensive guide to developing microservices with practical patterns for reliability and fault tolerance.', 520, N'Computers', N'http://books.google.com/books/content?id=7nO4AAAAQBAJ&printsec=frontcover&img=1&zoom=1', SYSUTCDATETIME());
    PRINT 'GoogleBooks seeded.';
END
GO

-- 3. Seed Countries
IF NOT EXISTS (SELECT 1 FROM dbo.Countries)
BEGIN
    INSERT INTO dbo.Countries (CommonName, OfficialName, Cca2, Cca3, Capital, Region, Subregion, Population, Area, FlagUrl, CreatedAt)
    VALUES 
    (N'Thailand', N'Kingdom of Thailand', N'TH', N'THA', N'Bangkok', N'Asia', N'South-Eastern Asia', 69799978, 513120, N'https://flagcdn.com/w320/th.png', SYSUTCDATETIME()),
    (N'Japan', N'Japan', N'JP', N'JPN', N'Tokyo', N'Asia', N'Eastern Asia', 125836021, 377930, N'https://flagcdn.com/w320/jp.png', SYSUTCDATETIME()),
    (N'United States', N'United States of America', N'US', N'USA', N'Washington, D.C.', N'Americas', N'North America', 329484123, 9372610, N'https://flagcdn.com/w320/us.png', SYSUTCDATETIME());
    PRINT 'Countries seeded.';
END
GO

-- 4. Seed CatFacts
IF NOT EXISTS (SELECT 1 FROM dbo.CatFacts)
BEGIN
    INSERT INTO dbo.CatFacts (Fact, Length, CreatedAt)
    VALUES 
    (N'Cats sleep for 70% of their lives.', 34, SYSUTCDATETIME()),
    (N'A group of cats is called a clowder.', 36, SYSUTCDATETIME()),
    (N'Cats have 32 muscles in each ear to control their outer ear flap.', 62, SYSUTCDATETIME());
    PRINT 'CatFacts seeded.';
END
GO

-- 5. Seed NasaApods
IF NOT EXISTS (SELECT 1 FROM dbo.NasaApods)
BEGIN
    INSERT INTO dbo.NasaApods (Date, Title, Explanation, Url, HdUrl, MediaType, Copyright, CreatedAt)
    VALUES 
    (N'2026-09-25', N'Nebula in the Constellation of Orion', N'The Orion Nebula is a picture-rich laboratory where astronomers study how stars are born.', N'https://apod.nasa.gov/apod/image/2609/orion_nebula.jpg', N'https://apod.nasa.gov/apod/image/2609/orion_nebula_hd.jpg', N'image', N'NASA / ESA', SYSUTCDATETIME()),
    (N'2026-09-26', N'Andromeda Galaxy in Deep Space', N'The Andromeda Galaxy is the closest major spiral galaxy to our Milky Way.', N'https://apod.nasa.gov/apod/image/2609/andromeda.jpg', N'https://apod.nasa.gov/apod/image/2609/andromeda_hd.jpg', N'image', N'NASA', SYSUTCDATETIME());
    PRINT 'NasaApods seeded.';
END
GO

-- 6. Seed Sample XmlRecords (Bonus 3)
IF NOT EXISTS (SELECT 1 FROM dbo.XmlRecords)
BEGIN
    INSERT INTO dbo.XmlRecords (RecordId, Name, Category, Value, Status, Description, RecordDate, RawXml, ImportedAt)
    VALUES 
    (N'XML-0001', N'Smart IoT Gateway v2', N'Electronics', 249.99, N'Active', N'Dual-band gateway module for smart sensor aggregation.', N'2026-09-26', N'<record id="XML-0001"><name>Smart IoT Gateway v2</name><category>Electronics</category><value>249.99</value><status>Active</status></record>', SYSUTCDATETIME()),
    (N'XML-0002', N'Cloud Edge Compute Box', N'Server Hardware', 899.50, N'Active', N'Edge computing unit with 16-core CPU.', N'2026-09-26', N'<record id="XML-0002"><name>Cloud Edge Compute Box</name><category>Server Hardware</category><value>899.50</value><status>Active</status></record>', SYSUTCDATETIME());
    PRINT 'XmlRecords seeded.';
END
GO

-- 7. Seed Sample JsonRecords (Bonus 4)
IF NOT EXISTS (SELECT 1 FROM dbo.JsonRecords)
BEGIN
    INSERT INTO dbo.JsonRecords (RecordId, Title, Category, Amount, Status, Description, RecordDate, RawJson, ImportedAt)
    VALUES 
    (N'JSON-0001', N'Azure Cloud VM Reservation (Option C)', N'Cloud Infrastructure', 1450.00, N'Completed', N'Provisioning Azure Virtual Machine for Group 03 backend deployment.', N'2026-09-26', N'{"id":"JSON-0001","title":"Azure Cloud VM Reservation (Option C)","category":"Cloud Infrastructure","amount":1450.00,"status":"Completed"}', SYSUTCDATETIME()),
    (N'JSON-0002', N'SSL Certificate & DuckDNS Domain', N'Network Security', 120.00, N'Completed', N'Configuring HTTPS and Custom Domain for g3-project.duckdns.org.', N'2026-09-26', N'{"id":"JSON-0002","title":"SSL Certificate & DuckDNS Domain","category":"Network Security","amount":120.00,"status":"Completed"}', SYSUTCDATETIME());
    PRINT 'JsonRecords seeded.';
END
GO

PRINT 'All seed data executed successfully.';
