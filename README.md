# G3 — Cloud-Based Backend Integration System
**รายวิชา 310-2203 | กำหนดส่ง: 29 กันยายน 2569 | กลุ่ม 3 | คะแนนเต็ม: 38 (33 ปกติ + 5 Bonus)**  
**Repository:** [https://github.com/Thanwarat6810301017/G3_Backend.git](https://github.com/Thanwarat6810301017/G3_Backend.git)

---

## 📌 บทบาทของ Member 5 – Database Architecture & Core API Specialist
**เป้าหมาย:** สนับสนุน Part B, Bonus 3, Bonus 4 และ Bonus 5 ให้กลุ่มได้รับคะแนนเต็ม

### งานที่ส่งมอบ (Deliverables):
1. **Database Scripts (.sql):**
   - [`Database/01_CreateSchema.sql`](file:///C:/Users/nitro/.gemini/antigravity/scratch/G3_Backend/Database/01_CreateSchema.sql): สคริปต์สร้าง Database `G3_Backend_DB`, ตารางทั้ง 7 ตาราง, คอนสเตรนต์ และ Non-clustered Indexes
   - [`Database/02_SeedData.sql`](file:///C:/Users/nitro/.gemini/antigravity/scratch/G3_Backend/Database/02_SeedData.sql): ข้อมูลเริ่มต้นสำหรับการทดสอบระบบทันทีหลัง Deploy
   - [`Database/03_StoredProcedures.sql`](file:///C:/Users/nitro/.gemini/antigravity/scratch/G3_Backend/Database/03_StoredProcedures.sql): Stored Procedures และ Views สำหรับการสรุปสถิติ Dashboard และ Bulk Insert XML
   - [`Database/04_VerifyData.sql`](file:///C:/Users/nitro/.gemini/antigravity/scratch/G3_Backend/Database/04_VerifyData.sql): สคริปต์ตรวจสอบความถูกต้องของข้อมูลทั้งหมด (สำหรับ Member 6 ใช้ตัดต่อวิดีโอเดโม)
2. **DB Context & Entity Framework Data Models:**
   - [`ApplicationDbContext.cs`](file:///C:/Users/nitro/.gemini/antigravity/scratch/G3_Backend/Project_G3_Group03/Data/ApplicationDbContext.cs)
   - Entity Models 7 ชุด ในโฟลเดอร์ `Project_G3_Group03/Models/Entities/`
3. **Core RESTful API Endpoints:**
   - คอนโทรลเลอร์ครบทั้ง 8 ตัว รองรับ CRUD, Save Data, และ Query
   - รองรับ Swagger UI (`/swagger`) สำหรับตรวจสอบและทดสอบ API ผ่าน Browser
4. **เอกสารสถาปัตยกรรมและการเชื่อมต่อ:**
   - [`database-architecture.md`](file:///C:/Users/nitro/.gemini/antigravity/scratch/G3_Backend/database-architecture.md): เอกสารอธิบายโครงสร้างตาราง, ERD Diagram, และ Indexing Design
   - [`api-documentation.md`](file:///C:/Users/nitro/.gemini/antigravity/scratch/G3_Backend/api-documentation.md): รายละเอียด Endpoint, Request/Response ตัวอย่าง และคู่มือการใช้งาน

---

## 🗂 โครงสร้างโปรเจกต์ (Project Structure)
```
G3_Backend/
├── Database/                               <-- รวมสคริปต์ SQL ทั้งหมดของ M5 สำหรับส่งมอบ
│   ├── 01_CreateSchema.sql
│   ├── 02_SeedData.sql
│   ├── 03_StoredProcedures.sql
│   └── 04_VerifyData.sql
├── Project_G3_Group03/                     <-- โฟลเดอร์โปรเจกต์ตามชื่อที่กำหนด
│   ├── Controllers/
│   │   ├── CatFactsController.cs           <-- Part B: Cat Facts API
│   │   ├── CountriesController.cs          <-- Part B: REST Countries API
│   │   ├── DashboardController.cs          <-- Bonus 5: Dashboard Metrics & Health Check
│   │   ├── GoogleBooksController.cs        <-- Part B: Google Books API
│   │   ├── JsonRecordsController.cs        <-- Bonus 4: JSON 1,000 Records Import
│   │   ├── NasaApodController.cs           <-- Part B: NASA APOD API
│   │   ├── OpenLibraryController.cs        <-- Part B: Open Library API
│   │   └── XmlRecordsController.cs         <-- Bonus 3: XML 1,000 Records Import
│   ├── Data/
│   │   ├── ApplicationDbContext.cs         <-- EF Core DbContext
│   │   └── DbInitializer.cs                <-- Auto Migration & Seed Data
│   ├── Models/
│   │   ├── DTOs/
│   │   │   ├── ApiResponseDto.cs
│   │   │   ├── DashboardSummaryDto.cs
│   │   │   ├── ImportResultDto.cs
│   │   │   └── PagedResultDto.cs
│   │   └── Entities/
│   │       ├── CatFact.cs
│   │       ├── GoogleBook.cs
│   │       ├── JsonRecord.cs
│   │       ├── NasaApod.cs
│   │       ├── OpenLibraryBook.cs
│   │       ├── RestCountry.cs
│   │       └── XmlRecord.cs
│   ├── Services/
│   │   ├── ExternalApiService.cs           <-- ฟังก์ชันบันทึกและเชื่อมต่อ 5 Public APIs
│   │   ├── IExternalApiService.cs
│   │   ├── JsonImportService.cs            <-- บริการ Bulk Import JSON 1,000 เรคคอร์ด
│   │   ├── IJsonImportService.cs
│   │   ├── XmlImportService.cs             <-- บริการ Bulk Import XML 1,000 เรคคอร์ด
│   │   └── IXmlImportService.cs
│   ├── Database/                           <-- สำเนา SQL Scripts ภายในโปรเจกต์
│   ├── appsettings.json
│   └── Program.cs
├── database-architecture.md                <-- เอกสารการออกแบบฐานข้อมูลฉบับเต็ม
├── api-documentation.md                    <-- เอกสาร API Specification ฉบับเต็ม
└── README.md
```

---

## 🚀 คู่มือการเชื่อมต่อสำหรับเพื่อนร่วมกลุ่ม (Team Integration Guide)

### 👤 Member 1 – Cloud & Infrastructure Lead
- **Azure VM Deployment:** 
  - รันคำสั่ง `dotnet run --project Project_G3_Group03` หรือ Publish ผ่าน IIS / Kestrel service
  - เปิด Port ให้บริการ (เช่น `5000` / `5001` / `443`) และผูกกับ DuckDNS: `https://g3-project.duckdns.org`
  - ทดสอบ Health Check ได้ที่: `https://g3-project.duckdns.org/api/Dashboard/health`
  - ตรวจสอบ Swagger UI ได้ที่: `https://g3-project.duckdns.org/swagger`

### 👤 Member 2 – XML Data Processing Specialist
- **Bonus 3 (Import XML 1,000 Records เข้า SQL Server):**
  - เมื่อ Member 2 สร้างไฟล์ `1000records.xml` เสร็จ สามารถเรียก API:
    - **Endpoint:** `POST /api/XmlRecords/import-file` (ส่งไฟล์ผ่าน `multipart/form-data`)
    - หรือส่ง Raw XML Body ผ่าน: `POST /api/XmlRecords/import-raw`
  - ระบบจะทำการ Parse และ Bulk Insert ข้อมูลทั้ง 1,000 รายการเข้าสู่ตาราง `XmlRecords` ใน SQL Server ทันที
  - สามารถทดสอบ Seed อัตโนมัติได้ที่: `POST /api/XmlRecords/seed-1000`

### 👤 Member 3 – JSON Data Processing & Dashboard Lead
- **Bonus 4 (Import JSON 1,000 Records เข้า SQL Server):**
  - เมื่อ Member 3 สร้างไฟล์ `1000records.json` เสร็จ สามารถเรียก API:
    - **Endpoint:** `POST /api/JsonRecords/import-file` (ส่งไฟล์ผ่าน `multipart/form-data`)
    - หรือส่ง Raw JSON Array ผ่าน: `POST /api/JsonRecords/import-raw`
  - ระบบจะทำการ Bulk Insert ข้อมูลทั้ง 1,000 รายการเข้าสู่ตาราง `JsonRecords` ใน SQL Server
  - สามารถทดสอบ Seed อัตโนมัติได้ที่: `POST /api/JsonRecords/seed-1000`
- **Bonus 5 (Dashboard UI):**
  - ดึงข้อมูลสถิติสรุปยอดรวมแบบเรียลไทม์ผ่าน: `GET /api/Dashboard/summary` เพื่อนำไปพล็อตชาร์ตหรือแสดง Card metrics บน Dashboard

### 👤 Member 4 – External API Integration Specialist
- **Part B (บันทึกข้อมูลจาก 5 Public APIs ลง SQL Server):**
  - เมื่อดึงข้อมูลจาก API ตัวใด สามารถส่ง Payload มาบันทึกผ่าน Endpoint:
    - `POST /api/OpenLibrary`
    - `POST /api/GoogleBooks`
    - `POST /api/Countries`
    - `POST /api/CatFacts`
    - `POST /api/NasaApod`
  - หรือสั่งให้ระบบดึงและบันทึกอัตโนมัติผ่าน Endpoint:
    - `POST /api/{Controller}/fetch-and-save`

### 👤 Member 6 – AI-Log Specialist, QA & Video Production
- โฟลเดอร์โปรเจกต์ถูกตั้งชื่อตรงตามข้อกำหนดคือ `Project_G3_Group03`
- สคริปต์ SQL ทั้งหมดอยู่ในโฟลเดอร์ `Database/` พร้อมใช้งานในคลิปเดโม (`demo.mp4`)
- สามารถรัน `Database/04_VerifyData.sql` เพื่อแสดงผลลัพธ์ข้อมูลทุกตารางบนหน้าจอตอนอัดวิดีโอได้ทันที

---

## 🛠 วิธีรันโปรเจกต์ในเครื่อง (Local Development)

1. ตรวจสอบเวอร์ชัน .NET:
   ```bash
   dotnet --version
   ```
2. เปิดโฟลเดอร์โปรเจกต์:
   ```bash
   cd Project_G3_Group03
   ```
3. รันโปรเจกต์:
   ```bash
   dotnet run
   ```
4. เปิดเบราว์เซอร์ไปที่:
   ```
   https://localhost:5001/swagger  หรือ  http://localhost:5000/swagger
   ```
