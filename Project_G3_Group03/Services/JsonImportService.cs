using System.Diagnostics;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Project_G3_Group03.Data;
using Project_G3_Group03.Models.DTOs;
using Project_G3_Group03.Models.Entities;

namespace Project_G3_Group03.Services;

public class JsonImportService : IJsonImportService
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<JsonImportService> _logger;

    public JsonImportService(ApplicationDbContext context, ILogger<JsonImportService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<ImportResultDto> ImportJsonFileAsync(Stream stream)
    {
        using var reader = new StreamReader(stream);
        var jsonContent = await reader.ReadToEndAsync();
        return await ImportJsonAsync(jsonContent);
    }

    public async Task<ImportResultDto> ImportJsonAsync(string jsonContent)
    {
        var stopwatch = Stopwatch.StartNew();
        var records = new List<JsonRecord>();
        int failedCount = 0;

        try
        {
            using var doc = JsonDocument.Parse(jsonContent);
            JsonElement arrayElement;

            if (doc.RootElement.ValueKind == JsonValueKind.Array)
            {
                arrayElement = doc.RootElement;
            }
            else if (doc.RootElement.TryGetProperty("records", out var rProp) && rProp.ValueKind == JsonValueKind.Array)
            {
                arrayElement = rProp;
            }
            else if (doc.RootElement.TryGetProperty("items", out var iProp) && iProp.ValueKind == JsonValueKind.Array)
            {
                arrayElement = iProp;
            }
            else if (doc.RootElement.TryGetProperty("data", out var dProp) && dProp.ValueKind == JsonValueKind.Array)
            {
                arrayElement = dProp;
            }
            else
            {
                return new ImportResultDto
                {
                    TotalCount = 0,
                    SuccessCount = 0,
                    FailedCount = 1,
                    ElapsedMilliseconds = stopwatch.ElapsedMilliseconds,
                    Message = "JSON root is not an array or does not contain a records/items/data array property."
                };
            }

            foreach (var el in arrayElement.EnumerateArray())
            {
                try
                {
                    var recordId = el.TryGetProperty("id", out var idProp) ? idProp.ToString() 
                                   : el.TryGetProperty("recordId", out var rIdProp) ? rIdProp.ToString() 
                                   : Guid.NewGuid().ToString()[..8];

                    var title = el.TryGetProperty("title", out var tProp) ? tProp.GetString() ?? "Unnamed JSON Record"
                                : el.TryGetProperty("name", out var nProp) ? nProp.GetString() ?? "Unnamed JSON Record"
                                : "Unnamed JSON Record";

                    var category = el.TryGetProperty("category", out var cProp) ? cProp.GetString() : "General";

                    decimal amount = 0;
                    if (el.TryGetProperty("amount", out var aProp))
                    {
                        if (aProp.ValueKind == JsonValueKind.Number) amount = aProp.GetDecimal();
                        else if (decimal.TryParse(aProp.GetString(), out var parsedA)) amount = parsedA;
                    }
                    else if (el.TryGetProperty("price", out var pProp))
                    {
                        if (pProp.ValueKind == JsonValueKind.Number) amount = pProp.GetDecimal();
                        else if (decimal.TryParse(pProp.GetString(), out var parsedP)) amount = parsedP;
                    }
                    else if (el.TryGetProperty("value", out var vProp))
                    {
                        if (vProp.ValueKind == JsonValueKind.Number) amount = vProp.GetDecimal();
                        else if (decimal.TryParse(vProp.GetString(), out var parsedV)) amount = parsedV;
                    }

                    var status = el.TryGetProperty("status", out var sProp) ? sProp.GetString() : "Active";
                    var description = el.TryGetProperty("description", out var descProp) ? descProp.GetString() : null;
                    var date = el.TryGetProperty("date", out var dtProp) ? dtProp.GetString() : DateTime.UtcNow.ToString("yyyy-MM-dd");

                    records.Add(new JsonRecord
                    {
                        RecordId = recordId,
                        Title = title,
                        Category = category,
                        Amount = amount,
                        Status = status,
                        Description = description,
                        RecordDate = date,
                        RawJson = el.ToString(),
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
                await _context.JsonRecords.AddRangeAsync(records);
                await _context.SaveChangesAsync();
            }

            stopwatch.Stop();
            return new ImportResultDto
            {
                TotalCount = records.Count + failedCount,
                SuccessCount = records.Count,
                FailedCount = failedCount,
                ElapsedMilliseconds = stopwatch.ElapsedMilliseconds,
                Message = $"Successfully imported {records.Count} JSON records into SQL Server in {stopwatch.ElapsedMilliseconds} ms."
            };
        }
        catch (Exception ex)
        {
            stopwatch.Stop();
            _logger.LogError(ex, "Failed to parse JSON content");
            return new ImportResultDto
            {
                TotalCount = 0,
                SuccessCount = 0,
                FailedCount = 1,
                ElapsedMilliseconds = stopwatch.ElapsedMilliseconds,
                Message = $"JSON Parse Error: {ex.Message}"
            };
        }
    }

    public async Task<ImportResultDto> Seed1000JsonRecordsAsync()
    {
        var stopwatch = Stopwatch.StartNew();
        var records = new List<JsonRecord>(1000);
        var categories = new[] { "Cloud Services", "Cybersecurity", "Database", "AI & ML", "DevOps", "Networking", "Mobile Apps", "Analytics" };
        var statuses = new[] { "Completed", "Pending", "Processing", "Cancelled", "Failed" };
        var random = new Random(101);

        for (int i = 1; i <= 1000; i++)
        {
            var cat = categories[random.Next(categories.Length)];
            var stat = statuses[random.Next(statuses.Length)];
            var amt = (decimal)Math.Round(random.NextDouble() * 1500 + 50, 2);

            records.Add(new JsonRecord
            {
                RecordId = $"JSON-{i:D4}",
                Title = $"Transaction {cat} #{i:D4}",
                Category = cat,
                Amount = amt,
                Status = stat,
                Description = $"System-generated test transaction payload for {cat} tier {stat}.",
                RecordDate = DateTime.UtcNow.AddDays(-random.Next(180)).ToString("yyyy-MM-dd"),
                RawJson = $"{{\"id\":\"JSON-{i:D4}\",\"title\":\"Transaction {cat} #{i:D4}\",\"category\":\"{cat}\",\"amount\":{amt},\"status\":\"{stat}\"}}",
                ImportedAt = DateTime.UtcNow
            });
        }

        await _context.JsonRecords.AddRangeAsync(records);
        await _context.SaveChangesAsync();
        stopwatch.Stop();

        return new ImportResultDto
        {
            TotalCount = 1000,
            SuccessCount = 1000,
            FailedCount = 0,
            ElapsedMilliseconds = stopwatch.ElapsedMilliseconds,
            Message = $"Successfully generated and imported 1,000 JSON records into SQL Server in {stopwatch.ElapsedMilliseconds} ms."
        };
    }

    public async Task<PagedResult<JsonRecord>> GetRecordsAsync(int pageNumber, int pageSize, string? search = null)
    {
        var query = _context.JsonRecords.AsNoTracking().AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(r => r.Title.Contains(search) 
                                  || r.RecordId.Contains(search) 
                                  || (r.Category != null && r.Category.Contains(search))
                                  || (r.Status != null && r.Status.Contains(search)));
        }

        var total = await query.CountAsync();
        var items = await query.OrderByDescending(r => r.Id)
                               .Skip((pageNumber - 1) * pageSize)
                               .Take(pageSize)
                               .ToListAsync();

        return new PagedResult<JsonRecord>
        {
            Items = items,
            TotalRecords = total,
            PageNumber = pageNumber,
            PageSize = pageSize
        };
    }

    public async Task<JsonRecord?> GetByIdAsync(int id)
    {
        return await _context.JsonRecords.FindAsync(id);
    }
}
