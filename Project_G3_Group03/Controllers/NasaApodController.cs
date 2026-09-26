using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project_G3_Group03.Data;
using Project_G3_Group03.Models.DTOs;
using Project_G3_Group03.Models.Entities;
using Project_G3_Group03.Services;

namespace Project_G3_Group03.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class NasaApodController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly IExternalApiService _apiService;

    public NasaApodController(ApplicationDbContext context, IExternalApiService apiService)
    {
        _context = context;
        _apiService = apiService;
    }

    /// <summary>
    /// Get all saved NASA APOD items from SQL Server (with optional pagination)
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<ApiResponse<PagedResult<NasaApod>>>> GetAll(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? search = null)
    {
        var query = _context.NasaApods.AsNoTracking().AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(a => a.Title.Contains(search) 
                                  || a.Date.Contains(search) 
                                  || (a.Explanation != null && a.Explanation.Contains(search)));
        }

        var total = await query.CountAsync();
        var items = await query.OrderByDescending(a => a.Date)
                               .Skip((page - 1) * pageSize)
                               .Take(pageSize)
                               .ToListAsync();

        var result = new PagedResult<NasaApod>
        {
            Items = items,
            TotalRecords = total,
            PageNumber = page,
            PageSize = pageSize
        };

        return Ok(ApiResponse<PagedResult<NasaApod>>.Ok(result));
    }

    /// <summary>
    /// Get a single saved NASA APOD item by ID
    /// </summary>
    [HttpGet("{id:int}")]
    public async Task<ActionResult<ApiResponse<NasaApod>>> GetById(int id)
    {
        var apod = await _context.NasaApods.FindAsync(id);
        if (apod == null) return NotFound(ApiResponse<NasaApod>.Fail($"NASA APOD with ID {id} not found"));
        return Ok(ApiResponse<NasaApod>.Ok(apod));
    }

    /// <summary>
    /// Save a NASA APOD payload manually into SQL Server (Sent by Member 4 External API Specialist or Frontend)
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<ApiResponse<NasaApod>>> SaveApod([FromBody] NasaApod apod)
    {
        if (!ModelState.IsValid) return BadRequest(ApiResponse<NasaApod>.Fail("Invalid model"));
        var saved = await _apiService.SaveNasaApodAsync(apod);
        return CreatedAtAction(nameof(GetById), new { id = saved.Id }, ApiResponse<NasaApod>.Ok(saved, "NASA APOD saved successfully"));
    }

    /// <summary>
    /// Fetch directly from NASA APOD API and save to SQL Server
    /// </summary>
    [HttpPost("fetch-and-save")]
    public async Task<ActionResult<ApiResponse<NasaApod>>> FetchAndSave([FromQuery] string? date = null)
    {
        try
        {
            var saved = await _apiService.FetchAndSaveNasaApodAsync(date);
            return Ok(ApiResponse<NasaApod>.Ok(saved, "Fetched and saved NASA APOD"));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<NasaApod>.Fail($"Error fetching NASA APOD API: {ex.Message}"));
        }
    }
}
