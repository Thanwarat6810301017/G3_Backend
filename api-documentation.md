# Core API Specification & Integration Guide
**รายวิชา 310-2203 Cloud-Based Backend Integration System**  
**กลุ่ม 3 (Group 03) | ผู้รับผิดชอบ: Member 5 – Database Architecture & Core API Specialist**  
**URL ระบบ (Azure VM):** `https://g3-project.duckdns.org/swagger`

---

## 1. ภาพรวมของ API (API Overview)
Core API พัฒนาด้วย **ASP.NET Core Web API (.NET 10)** โดยทำหน้าที่เป็น Application / Service Tier กลางสำหรับเชื่อมโยงฐานข้อมูล SQL Server เข้ากับ:
- 5 External Public APIs ของ Member 4
- XML Processing Service ของ Member 2 (Bonus 3)
- JSON Processing Service และ Dashboard UI ของ Member 3 (Bonus 4 & 5)
- ระบบโครงสร้างพื้นฐานบน Azure VM และ DuckDNS ของ Member 1

---

## 2. รายการ API Endpoints ทั้งหมด

### 2.1 Public APIs Data & Save Management (Part B)

#### [1] Open Library Books (`/api/OpenLibrary`)
| Method | Endpoint | คำอธิบาย |
|---|---|---|
| `GET` | `/api/OpenLibrary?page=1&pageSize=10&search={keyword}` | ดึงรายการหนังสือ Open Library ที่บันทึกไว้ใน SQL Server (พร้อมแบ่งหน้าและค้นหา) |
| `GET` | `/api/OpenLibrary/{id}` | ดึงข้อมูลหนังสือตามรหัส ID |
| `POST` | `/api/OpenLibrary` | บันทึกข้อมูลหนังสือที่ส่งมาจาก Member 4 หรือ Frontend ลง SQL Server |
| `POST` | `/api/OpenLibrary/fetch-and-save?query=clean+code&limit=5` | ดึงข้อมูลตรงจาก Open Library API และบันทึกเข้า SQL Server ทันที |

#### [2] Google Books (`/api/GoogleBooks`)
| Method | Endpoint | คำอธิบาย |
|---|---|---|
| `GET` | `/api/GoogleBooks?page=1&pageSize=10&search={keyword}` | ดึงรายการหนังสือ Google Books ที่บันทึกไว้ใน SQL Server (พร้อมแบ่งหน้าและค้นหา) |
| `GET` | `/api/GoogleBooks/{id}` | ดึงข้อมูล Google Book ตามรหัส ID |
| `POST` | `/api/GoogleBooks` | บันทึกข้อมูล Google Book จาก Member 4 หรือ Frontend ลง SQL Server |
| `POST` | `/api/GoogleBooks/fetch-and-save?query=architecture&limit=5` | ดึงข้อมูลตรงจาก Google Books API และบันทึกเข้า SQL Server ทันที |

#### [3] REST Countries (`/api/Countries`)
| Method | Endpoint | คำอธิบาย |
|---|---|---|
| `GET` | `/api/Countries?page=1&pageSize=10&search={keyword}` | ดึงรายการข้อมูลประเทศที่บันทึกไว้ใน SQL Server |
| `GET` | `/api/Countries/{id}` | ดึงข้อมูลประเทศตามรหัส ID |
| `POST` | `/api/Countries` | บันทึกข้อมูลประเทศจาก Member 4 หรือ Frontend ลง SQL Server |
| `POST` | `/api/Countries/fetch-and-save?name=thailand` | ดึงข้อมูลตรงจาก REST Countries API และบันทึกเข้า SQL Server ทันที |

#### [4] Cat Facts (`/api/CatFacts`)
| Method | Endpoint | คำอธิบาย |
|---|---|---|
| `GET` | `/api/CatFacts?page=1&pageSize=10` | ดึงรายการเกร็ดความรู้แมวที่บันทึกไว้ใน SQL Server |
| `GET` | `/api/CatFacts/{id}` | ดึง Cat Fact ตามรหัส ID |
| `POST` | `/api/CatFacts` | บันทึกข้อมูล Cat Fact ลง SQL Server |
| `POST` | `/api/CatFacts/fetch-and-save` | ดึงข้อเท็จจริงใหม่จาก Cat Facts API และบันทึกลง SQL Server ทันที |

#### [5] NASA Astronomy Picture of the Day (`/api/NasaApod`)
| Method | Endpoint | คำอธิบาย |
|---|---|---|
| `GET` | `/api/NasaApod?page=1&pageSize=10&search={keyword}` | ดึงข้อมูลภาพดาราศาสตร์ NASA APOD ที่บันทึกไว้ใน SQL Server |
| `GET` | `/api/NasaApod/{id}` | ดึง NASA APOD ตามรหัส ID |
| `POST` | `/api/NasaApod` | บันทึกข้อมูลภาพดาราศาสตร์ลง SQL Server |
| `POST` | `/api/NasaApod/fetch-and-save?date=2026-09-26` | ดึงภาพดาราศาสตร์ตรงจาก NASA API และบันทึกลง SQL Server ทันที |

---

### 2.2 XML Data Import & Management (Bonus 3)
Controller: `/api/XmlRecords`

| Method | Endpoint | รูปแบบ Payload | คำอธิบาย |
|---|---|---|---|
| `GET` | `/api/XmlRecords?page=1&pageSize=20&search={keyword}` | Query Parameters | ดึงข้อมูล XML 1,000 Records จาก SQL Server แบบแบ่งหน้า |
| `GET` | `/api/XmlRecords/{id}` | - | ดึงเรคคอร์ด XML ตามรหัส ID |
| `POST` | `/api/XmlRecords/import-file` | `multipart/form-data` (file: `1000records.xml`) | **[Bonus 3]** อัปโหลดและ Import ไฟล์ XML 1,000 Records ลง SQL Server |
| `POST` | `/api/XmlRecords/import-raw` | `application/xml` (Raw XML String) | Import ข้อมูล XML จาก Raw Body เข้าสู่ SQL Server |
| `POST` | `/api/XmlRecords/seed-1000` | - | ฟังก์ชันจำลองและ Import 1,000 XML Records อัตโนมัติเพื่อทดสอบ |

---

### 2.3 JSON Data Import & Management (Bonus 4)
Controller: `/api/JsonRecords`

| Method | Endpoint | รูปแบบ Payload | คำอธิบาย |
|---|---|---|---|
| `GET` | `/api/JsonRecords?page=1&pageSize=20&search={keyword}` | Query Parameters | ดึงข้อมูล JSON 1,000 Records จาก SQL Server แบบแบ่งหน้า |
| `GET` | `/api/JsonRecords/{id}` | - | ดึงเรคคอร์ด JSON ตามรหัส ID |
| `POST` | `/api/JsonRecords/import-file` | `multipart/form-data` (file: `1000records.json`) | **[Bonus 4]** อัปโหลดและ Import ไฟล์ JSON 1,000 Records ลง SQL Server |
| `POST` | `/api/JsonRecords/import-raw` | `application/json` (Raw JSON Array) | Import ข้อมูล JSON จาก Raw Body เข้าสู่ SQL Server |
| `POST` | `/api/JsonRecords/seed-1000` | - | ฟังก์ชันจำลองและ Import 1,000 JSON Records อัตโนมัติเพื่อทดสอบ |

---

### 2.4 Dashboard Summary & Health Check (Bonus 5 & Member 1)
Controller: `/api/Dashboard`

| Method | Endpoint | คำอธิบาย |
|---|---|---|
| `GET` | `/api/Dashboard/summary` | **[Bonus 5]** สรุปยอดข้อมูลรวมจาก 5 APIs และ XML/JSON 1,000 Records พร้อมประวัติกิจกรรมล่าสุด สำหรับ Dashboard UI ของ Member 3 |
| `GET` | `/api/Dashboard/health` | Health Check สำหรับตรวจสอบสถานะระบบ การเชื่อมต่อ Database และ Azure Deployment ของ Member 1 |

---

## 3. ตัวอย่าง Response Format

### 3.1 รูปแบบผลลัพธ์มาตรฐาน (Standard API Response Wrapper)
```json
{
  "success": true,
  "message": "Success",
  "data": { ... },
  "timestamp": "2026-09-26T16:15:00Z"
}
```

### 3.2 ตัวอย่าง Dashboard Summary (`GET /api/Dashboard/summary`)
```json
{
  "success": true,
  "message": "Success",
  "data": {
    "openLibraryCount": 15,
    "googleBooksCount": 12,
    "countriesCount": 25,
    "catFactsCount": 30,
    "nasaApodCount": 10,
    "xmlRecordsCount": 1000,
    "jsonRecordsCount": 1000,
    "totalSavedRecords": 2092,
    "databaseStatus": "Connected (Healthy)",
    "systemEnvironment": "Azure VM (Option C) - Group 03",
    "serverTimeUtc": "2026-09-26T16:15:00.1234567Z",
    "recentActivities": [
      {
        "source": "Open Library",
        "title": "Clean Code",
        "timestamp": "2026-09-26T16:10:00Z"
      },
      {
        "source": "XML Import",
        "title": "Smart IoT Gateway v2",
        "timestamp": "2026-09-26T16:12:00Z"
      }
    ]
  },
  "timestamp": "2026-09-26T16:15:00Z"
}
```
