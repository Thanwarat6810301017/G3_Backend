using Project_G3_Group03.Models.DTOs;
using Project_G3_Group03.Models.Entities;

namespace Project_G3_Group03.Services;

public interface IXmlImportService
{
    Task<ImportResultDto> ImportXmlAsync(string xmlContent);
    Task<ImportResultDto> ImportXmlFileAsync(Stream stream);
    Task<ImportResultDto> Seed1000XmlRecordsAsync();
    Task<PagedResult<XmlRecord>> GetRecordsAsync(int pageNumber, int pageSize, string? search = null);
    Task<XmlRecord?> GetByIdAsync(int id);
}
