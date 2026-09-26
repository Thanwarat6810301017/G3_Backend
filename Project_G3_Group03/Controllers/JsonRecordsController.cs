using Microsoft.AspNetCore.Mvc;
using Project_G3_Group03.Models.DTOs;
using Project_G3_Group03.Models.Entities;
using Project_G3_Group03.Services;

namespace Project_G3_Group03.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class JsonRecordsController : ControllerBase
{
    private readonly IJsonImportService _jsonService;

    public JsonRecordsController(IJsonImportService jsonService)
    {
        _jsonService = jsonService;
    }

    /// <summary>
    /// Get JSON records from SQL Server with pagination and search (Bonus 4)
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<ApiResponse<PagedResult<JsonRecord>>>> GetAll(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        [FromQuery] string? search = null)
    {
        var result = await _jsonService.GetRecordsAsync(page, pageSize, search);
        return Ok(ApiResponse<PagedResult<JsonRecord>>.Ok(result));
    }

    /// <summary>
    /// Get single JSON record by ID
    /// </summary>
    [HttpGet("{id:int}")]
    public async Task<ActionResult<ApiResponse<JsonRecord>>> GetById(int id)
    {
        var record = await _jsonService.GetByIdAsync(id);
        if (record == null) return NotFound(ApiResponse<JsonRecord>.Fail($"JSON record with ID {id} not found"));
        return Ok(ApiResponse<JsonRecord>.Ok(record));
    }

    /// <summary>
    /// Upload and import JSON file (up to 1,000+ records) into SQL Server (Bonus 4)
    /// </summary>
    [HttpPost("import-file")]
    [Consumes("multipart/form-data")]
    public async Task<ActionResult<ApiResponse<ImportResultDto>>> ImportJsonFile(IFormFile file)
    {
        if (file == null || file.Length == 0)
        {
            return BadRequest(ApiResponse<ImportResultDto>.Fail("No JSON file uploaded."));
        }

        using var stream = file.OpenReadStream();
        var result = await _jsonService.ImportJsonFileAsync(stream);
        return Ok(ApiResponse<ImportResultDto>.Ok(result, result.Message));
    }

    /// <summary>
    /// Import raw JSON string/array payload into SQL Server (Bonus 4)
    /// </summary>
    [HttpPost("import-raw")]
    [Consumes("application/json", "text/plain")]
    public async Task<ActionResult<ApiResponse<ImportResultDto>>> ImportJsonRaw()
    {
        using var reader = new StreamReader(Request.Body);
        var jsonContent = await reader.ReadToEndAsync();
        if (string.IsNullOrWhiteSpace(jsonContent))
        {
            return BadRequest(ApiResponse<ImportResultDto>.Fail("Empty JSON payload"));
        }

        var result = await _jsonService.ImportJsonAsync(jsonContent);
        return Ok(ApiResponse<ImportResultDto>.Ok(result, result.Message));
    }

    /// <summary>
    /// Auto-generate and batch import 1,000 JSON records into SQL Server (Bonus 4 verification)
    /// </summary>
    [HttpPost("seed-1000")]
    public async Task<ActionResult<ApiResponse<ImportResultDto>>> Seed1000()
    {
        var result = await _jsonService.Seed1000JsonRecordsAsync();
        return Ok(ApiResponse<ImportResultDto>.Ok(result, result.Message));
    }
}
