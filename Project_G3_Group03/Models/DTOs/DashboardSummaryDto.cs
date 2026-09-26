namespace Project_G3_Group03.Models.DTOs;

public class DashboardSummaryDto
{
    public int OpenLibraryCount { get; set; }
    public int GoogleBooksCount { get; set; }
    public int CountriesCount { get; set; }
    public int CatFactsCount { get; set; }
    public int NasaApodCount { get; set; }
    public int XmlRecordsCount { get; set; }
    public int JsonRecordsCount { get; set; }
    public int TotalSavedRecords => OpenLibraryCount + GoogleBooksCount + CountriesCount + CatFactsCount + NasaApodCount + XmlRecordsCount + JsonRecordsCount;

    public string DatabaseStatus { get; set; } = "Connected";
    public string SystemEnvironment { get; set; } = "Azure Virtual Machine (G3-Project)";
    public DateTime ServerTimeUtc { get; set; } = DateTime.UtcNow;

    public List<RecentActivityDto> RecentActivities { get; set; } = new();
}

public class RecentActivityDto
{
    public string Source { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public DateTime Timestamp { get; set; }
}

public class ImportResultDto
{
    public int TotalCount { get; set; }
    public int SuccessCount { get; set; }
    public int FailedCount { get; set; }
    public long ElapsedMilliseconds { get; set; }
    public string Message { get; set; } = string.Empty;
}
