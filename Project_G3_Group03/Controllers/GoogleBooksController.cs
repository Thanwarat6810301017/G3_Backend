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
public class GoogleBooksController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly IExternalApiService _apiService;

    public GoogleBooksController(ApplicationDbContext context, IExternalApiService apiService)
    {
        _context = context;
        _apiService = apiService;
    }

    /// <summary>
    /// Get all saved Google Books from SQL Server (with optional search and pagination)
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<ApiResponse<PagedResult<GoogleBook>>>> GetAll(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? search = null)
    {
        var query = _context.GoogleBooks.AsNoTracking().AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(b => b.Title.Contains(search) 
                                  || (b.Authors != null && b.Authors.Contains(search))
                                  || (b.Categories != null && b.Categories.Contains(search)));
        }

        var total = await query.CountAsync();
        var items = await query.OrderByDescending(b => b.Id)
                               .Skip((page - 1) * pageSize)
                               .Take(pageSize)
                               .ToListAsync();

        var result = new PagedResult<GoogleBook>
        {
            Items = items,
            TotalRecords = total,
            PageNumber = page,
            PageSize = pageSize
        };

        return Ok(ApiResponse<PagedResult<GoogleBook>>.Ok(result));
    }

    /// <summary>
    /// Get a single saved Google Book by ID
    /// </summary>
    [HttpGet("{id:int}")]
    public async Task<ActionResult<ApiResponse<GoogleBook>>> GetById(int id)
    {
        var book = await _context.GoogleBooks.FindAsync(id);
        if (book == null) return NotFound(ApiResponse<GoogleBook>.Fail($"Google Book with ID {id} not found"));
        return Ok(ApiResponse<GoogleBook>.Ok(book));
    }

    /// <summary>
    /// Save a Google Book payload manually into SQL Server (Sent by Member 4 External API Specialist or Frontend)
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<ApiResponse<GoogleBook>>> SaveBook([FromBody] GoogleBook book)
    {
        if (!ModelState.IsValid) return BadRequest(ApiResponse<GoogleBook>.Fail("Invalid model"));
        var saved = await _apiService.SaveGoogleBookAsync(book);
        return CreatedAtAction(nameof(GetById), new { id = saved.Id }, ApiResponse<GoogleBook>.Ok(saved, "Google Book saved successfully"));
    }

    /// <summary>
    /// Fetch directly from Google Books API and save to SQL Server
    /// </summary>
    [HttpPost("fetch-and-save")]
    public async Task<ActionResult<ApiResponse<List<GoogleBook>>>> FetchAndSave(
        [FromQuery] string query = "architecture",
        [FromQuery] int limit = 5)
    {
        try
        {
            var saved = await _apiService.FetchAndSaveGoogleBooksAsync(query, limit);
            return Ok(ApiResponse<List<GoogleBook>>.Ok(saved, $"Fetched and saved {saved.Count} books from Google Books API"));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<List<GoogleBook>>.Fail($"Error fetching Google Books API: {ex.Message}"));
        }
    }
}
