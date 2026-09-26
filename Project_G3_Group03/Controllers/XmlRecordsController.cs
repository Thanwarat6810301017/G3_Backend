using Microsoft.AspNetCore.Mvc;
using Project_G3_Group03.Models.DTOs;
using Project_G3_Group03.Models.Entities;
using Project_G3_Group03.Services;

namespace Project_G3_Group03.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class XmlRecordsController : ControllerBase
{
    private readonly IXmlImportService _xmlService;

    public XmlRecordsController(IXmlImportService xmlService)
    {
        _xmlService = xmlService;
    }

    /// <summary>
    /// Get XML records from SQL Server with pagination and search (Bonus 3)
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<ApiResponse<PagedResult<XmlRecord>>>> GetAll(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        [FromQuery] string? search = null)
    {
        var result = await _xmlService.GetRecordsAsync(page, pageSize, search);
        return Ok(ApiResponse<PagedResult<XmlRecord>>.Ok(result));
    }

    /// <summary>
    /// Get single XML record by ID
    /// </summary>
    [HttpGet("{id:int}")]
    public async Task<ActionResult<ApiResponse<XmlRecord>>> GetById(int id)
    {
        var record = await _xmlService.GetByIdAsync(id);
        if (record == null) return NotFound(ApiResponse<XmlRecord>.Fail($"XML record with ID {id} not found"));
        return Ok(ApiResponse<XmlRecord>.Ok(record));
    }

    /// <summary>
    /// Upload and import XML file (up to 1,000+ records) into SQL Server (Bonus 3)
    /// </summary>
    [HttpPost("import-file")]
    [Consumes("multipart/form-data")]
    public async Task<ActionResult<ApiResponse<ImportResultDto>>> ImportXmlFile(IFormFile file)
    {
        if (file == null || file.Length == 0)
        {
            return BadRequest(ApiResponse<ImportResultDto>.Fail("No XML file uploaded."));
        }

        using var stream = file.OpenReadStream();
        var result = await _xmlService.ImportXmlFileAsync(stream);
        return Ok(ApiResponse<ImportResultDto>.Ok(result, result.Message));
    }

    /// <summary>
    /// Import raw XML string payload into SQL Server (Bonus 3)
    /// </summary>
    [HttpPost("import-raw")]
    [Consumes("application/xml", "text/xml", "text/plain")]
    public async Task<ActionResult<ApiResponse<ImportResultDto>>> ImportXmlRaw()
    {
        using var reader = new StreamReader(Request.Body);
        var xmlContent = await reader.ReadToEndAsync();
        if (string.IsNullOrWhiteSpace(xmlContent))
        {
            return BadRequest(ApiResponse<ImportResultDto>.Fail("Empty XML payload"));
        }

        var result = await _xmlService.ImportXmlAsync(xmlContent);
        return Ok(ApiResponse<ImportResultDto>.Ok(result, result.Message));
    }

    /// <summary>
    /// Auto-generate and batch import 1,000 XML records into SQL Server (Bonus 3 verification)
    /// </summary>
    [HttpPost("seed-1000")]
    public async Task<ActionResult<ApiResponse<ImportResultDto>>> Seed1000()
    {
        var result = await _xmlService.Seed1000XmlRecordsAsync();
        return Ok(ApiResponse<ImportResultDto>.Ok(result, result.Message));
    }
}
