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
public class CountriesController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly IExternalApiService _apiService;

    public CountriesController(ApplicationDbContext context, IExternalApiService apiService)
    {
        _context = context;
        _apiService = apiService;
    }

    /// <summary>
    /// Get all saved Countries from SQL Server (with optional search and pagination)
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<ApiResponse<PagedResult<RestCountry>>>> GetAll(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? search = null)
    {
        var query = _context.Countries.AsNoTracking().AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(c => c.CommonName.Contains(search) 
                                  || (c.OfficialName != null && c.OfficialName.Contains(search))
                                  || (c.Capital != null && c.Capital.Contains(search))
                                  || (c.Region != null && c.Region.Contains(search)));
        }

        var total = await query.CountAsync();
        var items = await query.OrderByDescending(c => c.Id)
                               .Skip((page - 1) * pageSize)
                               .Take(pageSize)
                               .ToListAsync();

        var result = new PagedResult<RestCountry>
        {
            Items = items,
            TotalRecords = total,
            PageNumber = page,
            PageSize = pageSize
        };

        return Ok(ApiResponse<PagedResult<RestCountry>>.Ok(result));
    }

    /// <summary>
    /// Get a single saved country by ID
    /// </summary>
    [HttpGet("{id:int}")]
    public async Task<ActionResult<ApiResponse<RestCountry>>> GetById(int id)
    {
        var country = await _context.Countries.FindAsync(id);
        if (country == null) return NotFound(ApiResponse<RestCountry>.Fail($"Country with ID {id} not found"));
        return Ok(ApiResponse<RestCountry>.Ok(country));
    }

    /// <summary>
    /// Save a Country payload manually into SQL Server (Sent by Member 4 External API Specialist or Frontend)
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<ApiResponse<RestCountry>>> SaveCountry([FromBody] RestCountry country)
    {
        if (!ModelState.IsValid) return BadRequest(ApiResponse<RestCountry>.Fail("Invalid model"));
        var saved = await _apiService.SaveCountryAsync(country);
        return CreatedAtAction(nameof(GetById), new { id = saved.Id }, ApiResponse<RestCountry>.Ok(saved, "Country saved successfully"));
    }

    /// <summary>
    /// Fetch directly from REST Countries API and save to SQL Server
    /// </summary>
    [HttpPost("fetch-and-save")]
    public async Task<ActionResult<ApiResponse<List<RestCountry>>>> FetchAndSave([FromQuery] string name = "thailand")
    {
        try
        {
            var saved = await _apiService.FetchAndSaveCountriesAsync(name);
            return Ok(ApiResponse<List<RestCountry>>.Ok(saved, $"Fetched and saved {saved.Count} countries from REST Countries API"));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<List<RestCountry>>.Fail($"Error fetching REST Countries API: {ex.Message}"));
        }
    }
}
