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
public class CatFactsController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly IExternalApiService _apiService;

    public CatFactsController(ApplicationDbContext context, IExternalApiService apiService)
    {
        _context = context;
        _apiService = apiService;
    }

    /// <summary>
    /// Get all saved Cat Facts from SQL Server (with optional pagination)
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<ApiResponse<PagedResult<CatFact>>>> GetAll(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10)
    {
        var query = _context.CatFacts.AsNoTracking().OrderByDescending(c => c.Id);
        var total = await query.CountAsync();
        var items = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

        var result = new PagedResult<CatFact>
        {
            Items = items,
            TotalRecords = total,
            PageNumber = page,
            PageSize = pageSize
        };

        return Ok(ApiResponse<PagedResult<CatFact>>.Ok(result));
    }

    /// <summary>
    /// Get a single saved Cat Fact by ID
    /// </summary>
    [HttpGet("{id:int}")]
    public async Task<ActionResult<ApiResponse<CatFact>>> GetById(int id)
    {
        var fact = await _context.CatFacts.FindAsync(id);
        if (fact == null) return NotFound(ApiResponse<CatFact>.Fail($"Cat Fact with ID {id} not found"));
        return Ok(ApiResponse<CatFact>.Ok(fact));
    }

    /// <summary>
    /// Save a Cat Fact payload manually into SQL Server (Sent by Member 4 External API Specialist or Frontend)
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<ApiResponse<CatFact>>> SaveFact([FromBody] CatFact fact)
    {
        if (!ModelState.IsValid) return BadRequest(ApiResponse<CatFact>.Fail("Invalid model"));
        var saved = await _apiService.SaveCatFactAsync(fact);
        return CreatedAtAction(nameof(GetById), new { id = saved.Id }, ApiResponse<CatFact>.Ok(saved, "Cat Fact saved successfully"));
    }

    /// <summary>
    /// Fetch directly from Cat Facts API and save to SQL Server
    /// </summary>
    [HttpPost("fetch-and-save")]
    public async Task<ActionResult<ApiResponse<CatFact>>> FetchAndSave()
    {
        try
        {
            var saved = await _apiService.FetchAndSaveCatFactAsync();
            return Ok(ApiResponse<CatFact>.Ok(saved, "Fetched and saved cat fact from Cat Facts API"));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<CatFact>.Fail($"Error fetching Cat Facts API: {ex.Message}"));
        }
    }
}
