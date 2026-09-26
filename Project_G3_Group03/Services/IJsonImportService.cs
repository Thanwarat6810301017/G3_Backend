using Project_G3_Group03.Models.DTOs;
using Project_G3_Group03.Models.Entities;

namespace Project_G3_Group03.Services;

public interface IJsonImportService
{
    Task<ImportResultDto> ImportJsonAsync(string jsonContent);
    Task<ImportResultDto> ImportJsonFileAsync(Stream stream);
    Task<ImportResultDto> Seed1000JsonRecordsAsync();
    Task<PagedResult<JsonRecord>> GetRecordsAsync(int pageNumber, int pageSize, string? search = null);
    Task<JsonRecord?> GetByIdAsync(int id);
}
