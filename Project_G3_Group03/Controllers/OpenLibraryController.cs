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
public class OpenLibraryController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly IExternalApiService _apiService;

    public OpenLibraryController(ApplicationDbContext context, IExternalApiService apiService)
    {
        _context = context;
        _apiService = apiService;
    }

    /// <summary>
    /// Get all saved Open Library books from SQL Server (with optional search and pagination)
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<ApiResponse<PagedResult<OpenLibraryBook>>>> GetAll(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? search = null)
    {
        var query = _context.OpenLibraryBooks.AsNoTracking().AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(b => b.Title.Contains(search) 
                                  || (b.Author != null && b.Author.Contains(search))
                                  || (b.Isbn != null && b.Isbn.Contains(search)));
        }

        var total = await query.CountAsync();
        var items = await query.OrderByDescending(b => b.Id)
                               .Skip((page - 1) * pageSize)
                               .Take(pageSize)
                               .ToListAsync();

        var result = new PagedResult<OpenLibraryBook>
        {
            Items = items,
            TotalRecords = total,
            PageNumber = page,
            PageSize = pageSize
        };

        return Ok(ApiResponse<PagedResult<OpenLibraryBook>>.Ok(result));
    }

    /// <summary>
    /// Get a single saved Open Library book by ID
    /// </summary>
    [HttpGet("{id:int}")]
    public async Task<ActionResult<ApiResponse<OpenLibraryBook>>> GetById(int id)
    {
        var book = await _context.OpenLibraryBooks.FindAsync(id);
        if (book == null) return NotFound(ApiResponse<OpenLibraryBook>.Fail($"Book with ID {id} not found"));
        return Ok(ApiResponse<OpenLibraryBook>.Ok(book));
    }

    /// <summary>
    /// Save a book payload manually into SQL Server (Sent by Member 4 External API Specialist or Frontend)
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<ApiResponse<OpenLibraryBook>>> SaveBook([FromBody] OpenLibraryBook book)
    {
        if (!ModelState.IsValid) return BadRequest(ApiResponse<OpenLibraryBook>.Fail("Invalid model"));
        var saved = await _apiService.SaveOpenLibraryBookAsync(book);
        return CreatedAtAction(nameof(GetById), new { id = saved.Id }, ApiResponse<OpenLibraryBook>.Ok(saved, "Book saved successfully"));
    }

    /// <summary>
    /// Fetch directly from Open Library API and save to SQL Server
    /// </summary>
    [HttpPost("fetch-and-save")]
    public async Task<ActionResult<ApiResponse<List<OpenLibraryBook>>>> FetchAndSave(
        [FromQuery] string query = "clean code",
        [FromQuery] int limit = 5)
    {
        try
        {
            var saved = await _apiService.FetchAndSaveOpenLibraryAsync(query, limit);
            return Ok(ApiResponse<List<OpenLibraryBook>>.Ok(saved, $"Fetched and saved {saved.Count} books from Open Library API"));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<List<OpenLibraryBook>>.Fail($"Error fetching Open Library API: {ex.Message}"));
        }
    }
}
