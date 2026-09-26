using Project_G3_Group03.Models.Entities;

namespace Project_G3_Group03.Data;

public static class DbInitializer
{
    public static void Initialize(ApplicationDbContext context)
    {
        context.Database.EnsureCreated();

        // 1. Seed OpenLibraryBooks if empty
        if (!context.OpenLibraryBooks.Any())
        {
            context.OpenLibraryBooks.AddRange(
                new OpenLibraryBook
                {
                    WorkKey = "/works/OL45804W",
                    Title = "Clean Code: A Handbook of Agile Software Craftsmanship",
                    Author = "Robert C. Martin",
                    FirstPublishYear = 2008,
                    Isbn = "9780132350884",
                    CoverUrl = "https://covers.openlibrary.org/b/id/8231991-M.jpg",
                    CreatedAt = DateTime.UtcNow
                },
                new OpenLibraryBook
                {
                    WorkKey = "/works/OL27479W",
                    Title = "The Pragmatic Programmer",
                    Author = "Andrew Hunt, David Thomas",
                    FirstPublishYear = 1999,
                    Isbn = "9780201616224",
                    CoverUrl = "https://covers.openlibrary.org/b/id/8091016-M.jpg",
                    CreatedAt = DateTime.UtcNow
                }
            );
        }

        // 2. Seed GoogleBooks if empty
        if (!context.GoogleBooks.Any())
        {
            context.GoogleBooks.AddRange(
                new GoogleBook
                {
                    GoogleId = "zyTCAlFPjgYC",
                    Title = "Design Patterns: Elements of Reusable Object-Oriented Software",
                    Authors = "Erich Gamma, Richard Helm, Ralph Johnson, John Vlissides",
                    Publisher = "Addison-Wesley Professional",
                    PublishedDate = "1994-10-31",
                    Description = "Capturing a wealth of experience about the design of object-oriented software, four top-notch designers present a catalog of simple and succinct solutions to commonly occurring design problems.",
                    PageCount = 395,
                    Categories = "Computers",
                    ThumbnailUrl = "http://books.google.com/books/content?id=zyTCAlFPjgYC&printsec=frontcover&img=1&zoom=1",
                    CreatedAt = DateTime.UtcNow
                }
            );
        }

        // 3. Seed Countries if empty
        if (!context.Countries.Any())
        {
            context.Countries.AddRange(
                new RestCountry
                {
                    CommonName = "Thailand",
                    OfficialName = "Kingdom of Thailand",
                    Cca2 = "TH",
                    Cca3 = "THA",
                    Capital = "Bangkok",
                    Region = "Asia",
                    Subregion = "South-Eastern Asia",
                    Population = 69799978,
                    Area = 513120,
                    FlagUrl = "https://flagcdn.com/w320/th.png",
                    CreatedAt = DateTime.UtcNow
                },
                new RestCountry
                {
                    CommonName = "Japan",
                    OfficialName = "Japan",
                    Cca2 = "JP",
                    Cca3 = "JPN",
                    Capital = "Tokyo",
                    Region = "Asia",
                    Subregion = "Eastern Asia",
                    Population = 125836021,
                    Area = 377930,
                    FlagUrl = "https://flagcdn.com/w320/jp.png",
                    CreatedAt = DateTime.UtcNow
                }
            );
        }

        // 4. Seed CatFacts if empty
        if (!context.CatFacts.Any())
        {
            context.CatFacts.AddRange(
                new CatFact
                {
                    Fact = "Cats sleep for 70% of their lives.",
                    Length = 34,
                    CreatedAt = DateTime.UtcNow
                },
                new CatFact
                {
                    Fact = "A group of cats is called a clowder.",
                    Length = 36,
                    CreatedAt = DateTime.UtcNow
                }
            );
        }

        // 5. Seed NasaApods if empty
        if (!context.NasaApods.Any())
        {
            context.NasaApods.AddRange(
                new NasaApod
                {
                    Date = "2026-09-25",
                    Title = "Nebula in the Constellation of Orion",
                    Explanation = "The Orion Nebula is a picture-rich laboratory where astronomers study how stars are born.",
                    Url = "https://apod.nasa.gov/apod/image/2609/orion_nebula.jpg",
                    HdUrl = "https://apod.nasa.gov/apod/image/2609/orion_nebula_hd.jpg",
                    MediaType = "image",
                    Copyright = "NASA / ESA",
                    CreatedAt = DateTime.UtcNow
                }
            );
        }

        // 6. Seed sample XML records (Bonus 3)
        if (!context.XmlRecords.Any())
        {
            context.XmlRecords.AddRange(
                new XmlRecord
                {
                    RecordId = "XML-0001",
                    Name = "Sample XML Item 1",
                    Category = "Electronics",
                    Value = 199.99m,
                    Status = "Active",
                    Description = "Seeded sample XML record representing parsed XML structure.",
                    RecordDate = "2026-09-26",
                    ImportedAt = DateTime.UtcNow
                },
                new XmlRecord
                {
                    RecordId = "XML-0002",
                    Name = "Sample XML Item 2",
                    Category = "Office",
                    Value = 49.50m,
                    Status = "Pending",
                    Description = "Seeded sample XML record.",
                    RecordDate = "2026-09-26",
                    ImportedAt = DateTime.UtcNow
                }
            );
        }

        // 7. Seed sample JSON records (Bonus 4)
        if (!context.JsonRecords.Any())
        {
            context.JsonRecords.AddRange(
                new JsonRecord
                {
                    RecordId = "JSON-0001",
                    Title = "Sample JSON Item 1",
                    Category = "Software",
                    Amount = 299.00m,
                    Status = "Completed",
                    Description = "Seeded sample JSON record representing parsed JSON structure.",
                    RecordDate = "2026-09-26",
                    ImportedAt = DateTime.UtcNow
                },
                new JsonRecord
                {
                    RecordId = "JSON-0002",
                    Title = "Sample JSON Item 2",
                    Category = "Hardware",
                    Amount = 89.90m,
                    Status = "Shipped",
                    Description = "Seeded sample JSON record.",
                    RecordDate = "2026-09-26",
                    ImportedAt = DateTime.UtcNow
                }
            );
        }

        context.SaveChanges();
    }
}
