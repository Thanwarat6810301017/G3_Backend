using System.Diagnostics;
using System.Xml.Linq;
using Microsoft.EntityFrameworkCore;
using Project_G3_Group03.Data;
using Project_G3_Group03.Models.DTOs;
using Project_G3_Group03.Models.Entities;

namespace Project_G3_Group03.Services;

public class XmlImportService : IXmlImportService
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<XmlImportService> _logger;

    public XmlImportService(ApplicationDbContext context, ILogger<XmlImportService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<ImportResultDto> ImportXmlFileAsync(Stream stream)
    {
        using var reader = new StreamReader(stream);
        var xmlContent = await reader.ReadToEndAsync();
        return await ImportXmlAsync(xmlContent);
    }

    public async Task<ImportResultDto> ImportXmlAsync(string xmlContent)
    {
        var stopwatch = Stopwatch.StartNew();
        var records = new List<XmlRecord>();
        int failedCount = 0;

        try
        {
            var doc = XDocument.Parse(xmlContent);
            // Flexible root/record detection: <records><record>... or <items><item>... or <root><row>...
            var elements = doc.Root?.Elements() ?? Enumerable.Empty<XElement>();

            foreach (var el in elements)
            {
                try
                {
                    var recordId = el.Element("RecordId")?.Value 
                                   ?? el.Element("id")?.Value 
                                   ?? el.Element("Id")?.Value 
                                   ?? el.Attribute("id")?.Value 
                                   ?? Guid.NewGuid().ToString()[..8];

                    var name = el.Element("Name")?.Value 
                               ?? el.Element("Title")?.Value 
                               ?? el.Element("name")?.Value 
                               ?? "Unnamed XML Record";

                    var category = el.Element("Category")?.Value ?? el.Element("category")?.Value ?? "General";
                    
                    var valStr = el.Element("Value")?.Value ?? el.Element("Price")?.Value ?? el.Element("value")?.Value ?? "0";
                    decimal.TryParse(valStr, out var value);

                    var status = el.Element("Status")?.Value ?? el.Element("status")?.Value ?? "Active";
                    var desc = el.Element("Description")?.Value ?? el.Element("description")?.Value;
                    var date = el.Element("Date")?.Value ?? el.Element("RecordDate")?.Value ?? DateTime.UtcNow.ToString("yyyy-MM-dd");

                    records.Add(new XmlRecord
                    {
                        RecordId = recordId,
                        Name = name,
                        Category = category,
                        Value = value,
                        Status = status,
                        Description = desc,
                        RecordDate = date,
                        RawXml = el.ToString(),
                        ImportedAt = DateTime.UtcNow
                    });
                }
                catch
                {
                    failedCount++;
                }
            }

            if (records.Count > 0)
            {
                // Batch insert into database
                await _context.XmlRecords.AddRangeAsync(records);
                await _context.SaveChangesAsync();
            }

            stopwatch.Stop();
            return new ImportResultDto
            {
                TotalCount = records.Count + failedCount,
                SuccessCount = records.Count,
                FailedCount = failedCount,
                ElapsedMilliseconds = stopwatch.ElapsedMilliseconds,
                Message = $"Successfully imported {records.Count} XML records into SQL Server in {stopwatch.ElapsedMilliseconds} ms."
            };
        }
        catch (Exception ex)
        {
            stopwatch.Stop();
            _logger.LogError(ex, "Failed to parse XML content");
            return new ImportResultDto
            {
                TotalCount = 0,
                SuccessCount = 0,
                FailedCount = 1,
                ElapsedMilliseconds = stopwatch.ElapsedMilliseconds,
                Message = $"XML Parse Error: {ex.Message}"
            };
        }
    }

    public async Task<ImportResultDto> Seed1000XmlRecordsAsync()
    {
        var stopwatch = Stopwatch.StartNew();
        var records = new List<XmlRecord>(1000);
        var categories = new[] { "Electronics", "Books", "Clothing", "Home & Garden", "Sports", "Health", "Automotive", "Toys" };
        var statuses = new[] { "In Stock", "Out of Stock", "Discontinued", "Pre-order" };
        var random = new Random(42);

        for (int i = 1; i <= 1000; i++)
        {
            var cat = categories[random.Next(categories.Length)];
            var stat = statuses[random.Next(statuses.Length)];
            var val = (decimal)Math.Round(random.NextDouble() * 500 + 10, 2);

            records.Add(new XmlRecord
            {
                RecordId = $"XML-{i:D4}",
                Name = $"Product {cat} SKU-{i:D4}",
                Category = cat,
                Value = val,
                Status = stat,
                Description = $"Detailed XML description for item #{i} in category {cat}.",
                RecordDate = DateTime.UtcNow.AddDays(-random.Next(365)).ToString("yyyy-MM-dd"),
                RawXml = $"<record id=\"XML-{i:D4}\"><name>Product {cat} SKU-{i:D4}</name><category>{cat}</category><value>{val}</value><status>{stat}</status></record>",
                ImportedAt = DateTime.UtcNow
            });
        }

        await _context.XmlRecords.AddRangeAsync(records);
        await _context.SaveChangesAsync();
        stopwatch.Stop();

        return new ImportResultDto
        {
            TotalCount = 1000,
            SuccessCount = 1000,
            FailedCount = 0,
            ElapsedMilliseconds = stopwatch.ElapsedMilliseconds,
            Message = $"Successfully generated and imported 1,000 XML records into SQL Server in {stopwatch.ElapsedMilliseconds} ms."
        };
    }

    public async Task<PagedResult<XmlRecord>> GetRecordsAsync(int pageNumber, int pageSize, string? search = null)
    {
        var query = _context.XmlRecords.AsNoTracking().AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(r => r.Name.Contains(search) 
                                  || r.RecordId.Contains(search) 
                                  || (r.Category != null && r.Category.Contains(search))
                                  || (r.Status != null && r.Status.Contains(search)));
        }

        var total = await query.CountAsync();
        var items = await query.OrderByDescending(r => r.Id)
                               .Skip((pageNumber - 1) * pageSize)
                               .Take(pageSize)
                               .ToListAsync();

        return new PagedResult<XmlRecord>
        {
            Items = items,
            TotalRecords = total,
            PageNumber = pageNumber,
            PageSize = pageSize
        };
    }

    public async Task<XmlRecord?> GetByIdAsync(int id)
    {
        return await _context.XmlRecords.FindAsync(id);
    }
}
