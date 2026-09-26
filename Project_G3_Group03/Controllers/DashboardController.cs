using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project_G3_Group03.Data;
using Project_G3_Group03.Models.DTOs;

namespace Project_G3_Group03.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class DashboardController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public DashboardController(ApplicationDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Get aggregated metrics and summary for Member 3's Dashboard UI (Bonus 5)
    /// </summary>
    [HttpGet("summary")]
    public async Task<ActionResult<ApiResponse<DashboardSummaryDto>>> GetSummary()
    {
        var openLibCount = await _context.OpenLibraryBooks.CountAsync();
        var googleCount = await _context.GoogleBooks.CountAsync();
        var countryCount = await _context.Countries.CountAsync();
        var catCount = await _context.CatFacts.CountAsync();
        var apodCount = await _context.NasaApods.CountAsync();
        var xmlCount = await _context.XmlRecords.CountAsync();
        var jsonCount = await _context.JsonRecords.CountAsync();

        var recentActivities = new List<RecentActivityDto>();

        var latestBook = await _context.OpenLibraryBooks.OrderByDescending(b => b.Id).FirstOrDefaultAsync();
        if (latestBook != null)
        {
            recentActivities.Add(new RecentActivityDto
            {
                Source = "Open Library",
                Title = latestBook.Title,
                Timestamp = latestBook.CreatedAt
            });
        }

        var latestGoogle = await _context.GoogleBooks.OrderByDescending(b => b.Id).FirstOrDefaultAsync();
        if (latestGoogle != null)
        {
            recentActivities.Add(new RecentActivityDto
            {
                Source = "Google Books",
                Title = latestGoogle.Title,
                Timestamp = latestGoogle.CreatedAt
            });
        }

        var latestCountry = await _context.Countries.OrderByDescending(c => c.Id).FirstOrDefaultAsync();
        if (latestCountry != null)
        {
            recentActivities.Add(new RecentActivityDto
            {
                Source = "REST Countries",
                Title = latestCountry.CommonName,
                Timestamp = latestCountry.CreatedAt
            });
        }

        var latestXml = await _context.XmlRecords.OrderByDescending(x => x.Id).FirstOrDefaultAsync();
        if (latestXml != null)
        {
            recentActivities.Add(new RecentActivityDto
            {
                Source = "XML Import",
                Title = latestXml.Name,
                Timestamp = latestXml.ImportedAt
            });
        }

        var latestJson = await _context.JsonRecords.OrderByDescending(j => j.Id).FirstOrDefaultAsync();
        if (latestJson != null)
        {
            recentActivities.Add(new RecentActivityDto
            {
                Source = "JSON Import",
                Title = latestJson.Title,
                Timestamp = latestJson.ImportedAt
            });
        }

        var summary = new DashboardSummaryDto
        {
            OpenLibraryCount = openLibCount,
            GoogleBooksCount = googleCount,
            CountriesCount = countryCount,
            CatFactsCount = catCount,
            NasaApodCount = apodCount,
            XmlRecordsCount = xmlCount,
            JsonRecordsCount = jsonCount,
            DatabaseStatus = "Connected (Healthy)",
            SystemEnvironment = "Azure VM (Option C) - Group 03",
            ServerTimeUtc = DateTime.UtcNow,
            RecentActivities = recentActivities
        };

        return Ok(ApiResponse<DashboardSummaryDto>.Ok(summary));
    }

    /// <summary>
    /// Health check endpoint for Azure deployment & DuckDNS verification (Member 1)
    /// </summary>
    [HttpGet("health")]
    public ActionResult<ApiResponse<object>> Health()
    {
        return Ok(ApiResponse<object>.Ok(new
        {
            Status = "Healthy",
            Service = "G3 Cloud-Based Backend Integration System",
            Domain = "g3-project.duckdns.org",
            Course = "310-2203",
            Group = "Group 3",
            Role = "Member 5 – Database Architecture & Core API Specialist",
            ServerTime = DateTime.UtcNow
        }));
    }
}
