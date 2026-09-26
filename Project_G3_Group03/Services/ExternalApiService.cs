using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Project_G3_Group03.Data;
using Project_G3_Group03.Models.Entities;

namespace Project_G3_Group03.Services;

public class ExternalApiService : IExternalApiService
{
    private readonly HttpClient _httpClient;
    private readonly ApplicationDbContext _context;
    private readonly ILogger<ExternalApiService> _logger;

    public ExternalApiService(HttpClient httpClient, ApplicationDbContext context, ILogger<ExternalApiService> logger)
    {
        _httpClient = httpClient;
        _context = context;
        _logger = logger;
        _httpClient.DefaultRequestHeaders.Add("User-Agent", "G3-Backend-Integration-System/1.0");
    }

    #region 1. Open Library API
    public async Task<List<OpenLibraryBook>> FetchAndSaveOpenLibraryAsync(string query, int limit = 10)
    {
        var url = $"https://openlibrary.org/search.json?q={Uri.EscapeDataString(query)}&limit={limit}";
        var response = await _httpClient.GetAsync(url);
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();
        using var doc = JsonDocument.Parse(json);
        var docs = doc.RootElement.GetProperty("docs");

        var books = new List<OpenLibraryBook>();
        foreach (var element in docs.EnumerateArray())
        {
            var title = element.TryGetProperty("title", out var tProp) ? tProp.GetString() ?? "Unknown" : "Unknown";
            var workKey = element.TryGetProperty("key", out var kProp) ? kProp.GetString() ?? "" : "";
            
            string? author = null;
            if (element.TryGetProperty("author_name", out var aProp) && aProp.ValueKind == JsonValueKind.Array && aProp.GetArrayLength() > 0)
            {
                author = aProp[0].GetString();
            }

            int? year = element.TryGetProperty("first_publish_year", out var yProp) && yProp.TryGetInt32(out var yVal) ? yVal : null;
            
            string? isbn = null;
            if (element.TryGetProperty("isbn", out var iProp) && iProp.ValueKind == JsonValueKind.Array && iProp.GetArrayLength() > 0)
            {
                isbn = iProp[0].GetString();
            }

            string? coverUrl = null;
            if (element.TryGetProperty("cover_i", out var cProp) && cProp.TryGetInt64(out var covId))
            {
                coverUrl = $"https://covers.openlibrary.org/b/id/{covId}-M.jpg";
            }

            var book = new OpenLibraryBook
            {
                WorkKey = workKey,
                Title = title,
                Author = author,
                FirstPublishYear = year,
                Isbn = isbn,
                CoverUrl = coverUrl,
                RawJson = element.ToString(),
                CreatedAt = DateTime.UtcNow
            };

            books.Add(book);
            _context.OpenLibraryBooks.Add(book);
        }

        await _context.SaveChangesAsync();
        return books;
    }

    public async Task<OpenLibraryBook> SaveOpenLibraryBookAsync(OpenLibraryBook book)
    {
        book.CreatedAt = DateTime.UtcNow;
        _context.OpenLibraryBooks.Add(book);
        await _context.SaveChangesAsync();
        return book;
    }
    #endregion

    #region 2. Google Books API
    public async Task<List<GoogleBook>> FetchAndSaveGoogleBooksAsync(string query, int limit = 10)
    {
        var url = $"https://www.googleapis.com/books/v1/volumes?q={Uri.EscapeDataString(query)}&maxResults={Math.Min(limit, 40)}";
        var response = await _httpClient.GetAsync(url);
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();
        using var doc = JsonDocument.Parse(json);

        var books = new List<GoogleBook>();
        if (!doc.RootElement.TryGetProperty("items", out var items))
        {
            return books;
        }

        foreach (var item in items.EnumerateArray())
        {
            var googleId = item.TryGetProperty("id", out var idProp) ? idProp.GetString() ?? "" : "";
            if (!item.TryGetProperty("volumeInfo", out var vInfo)) continue;

            var title = vInfo.TryGetProperty("title", out var tProp) ? tProp.GetString() ?? "Unknown" : "Unknown";
            
            string? authors = null;
            if (vInfo.TryGetProperty("authors", out var aProp) && aProp.ValueKind == JsonValueKind.Array)
            {
                authors = string.Join(", ", aProp.EnumerateArray().Select(a => a.GetString()));
            }

            var publisher = vInfo.TryGetProperty("publisher", out var pProp) ? pProp.GetString() : null;
            var publishedDate = vInfo.TryGetProperty("publishedDate", out var pdProp) ? pdProp.GetString() : null;
            var description = vInfo.TryGetProperty("description", out var dProp) ? dProp.GetString() : null;
            int? pageCount = vInfo.TryGetProperty("pageCount", out var pcProp) && pcProp.TryGetInt32(out var pcVal) ? pcVal : null;
            
            string? categories = null;
            if (vInfo.TryGetProperty("categories", out var cProp) && cProp.ValueKind == JsonValueKind.Array)
            {
                categories = string.Join(", ", cProp.EnumerateArray().Select(c => c.GetString()));
            }

            string? thumbnail = null;
            if (vInfo.TryGetProperty("imageLinks", out var imgProp) && imgProp.TryGetProperty("thumbnail", out var thumbProp))
            {
                thumbnail = thumbProp.GetString();
            }

            var book = new GoogleBook
            {
                GoogleId = googleId,
                Title = title,
                Authors = authors,
                Publisher = publisher,
                PublishedDate = publishedDate,
                Description = description,
                PageCount = pageCount,
                Categories = categories,
                ThumbnailUrl = thumbnail,
                RawJson = item.ToString(),
                CreatedAt = DateTime.UtcNow
            };

            books.Add(book);
            _context.GoogleBooks.Add(book);
        }

        await _context.SaveChangesAsync();
        return books;
    }

    public async Task<GoogleBook> SaveGoogleBookAsync(GoogleBook book)
    {
        book.CreatedAt = DateTime.UtcNow;
        _context.GoogleBooks.Add(book);
        await _context.SaveChangesAsync();
        return book;
    }
    #endregion

    #region 3. REST Countries API
    public async Task<List<RestCountry>> FetchAndSaveCountriesAsync(string query)
    {
        var url = string.IsNullOrWhiteSpace(query)
            ? "https://restcountries.com/v3.1/all?fields=name,cca2,cca3,capital,region,subregion,population,area,flags"
            : $"https://restcountries.com/v3.1/name/{Uri.EscapeDataString(query)}?fields=name,cca2,cca3,capital,region,subregion,population,area,flags";

        var response = await _httpClient.GetAsync(url);
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();
        using var doc = JsonDocument.Parse(json);

        var countries = new List<RestCountry>();
        foreach (var item in doc.RootElement.EnumerateArray())
        {
            var nameObj = item.GetProperty("name");
            var commonName = nameObj.GetProperty("common").GetString() ?? "";
            var officialName = nameObj.TryGetProperty("official", out var oProp) ? oProp.GetString() : null;
            var cca2 = item.TryGetProperty("cca2", out var c2) ? c2.GetString() : null;
            var cca3 = item.TryGetProperty("cca3", out var c3) ? c3.GetString() : null;
            
            string? capital = null;
            if (item.TryGetProperty("capital", out var capProp) && capProp.ValueKind == JsonValueKind.Array && capProp.GetArrayLength() > 0)
            {
                capital = capProp[0].GetString();
            }

            var region = item.TryGetProperty("region", out var rProp) ? rProp.GetString() : null;
            var subregion = item.TryGetProperty("subregion", out var srProp) ? srProp.GetString() : null;
            long? population = item.TryGetProperty("population", out var popProp) && popProp.TryGetInt64(out var pVal) ? pVal : null;
            double? area = item.TryGetProperty("area", out var arProp) && arProp.TryGetDouble(out var aVal) ? aVal : null;
            
            string? flagUrl = null;
            if (item.TryGetProperty("flags", out var fProp) && fProp.TryGetProperty("png", out var pngProp))
            {
                flagUrl = pngProp.GetString();
            }

            var country = new RestCountry
            {
                CommonName = commonName,
                OfficialName = officialName,
                Cca2 = cca2,
                Cca3 = cca3,
                Capital = capital,
                Region = region,
                Subregion = subregion,
                Population = population,
                Area = area,
                FlagUrl = flagUrl,
                RawJson = item.ToString(),
                CreatedAt = DateTime.UtcNow
            };

            countries.Add(country);
            _context.Countries.Add(country);
        }

        await _context.SaveChangesAsync();
        return countries;
    }

    public async Task<RestCountry> SaveCountryAsync(RestCountry country)
    {
        country.CreatedAt = DateTime.UtcNow;
        _context.Countries.Add(country);
        await _context.SaveChangesAsync();
        return country;
    }
    #endregion

    #region 4. Cat Facts API
    public async Task<CatFact> FetchAndSaveCatFactAsync()
    {
        var url = "https://catfact.ninja/fact";
        var response = await _httpClient.GetAsync(url);
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();
        using var doc = JsonDocument.Parse(json);

        var factText = doc.RootElement.GetProperty("fact").GetString() ?? "";
        var length = doc.RootElement.GetProperty("length").GetInt32();

        var fact = new CatFact
        {
            Fact = factText,
            Length = length,
            CreatedAt = DateTime.UtcNow
        };

        _context.CatFacts.Add(fact);
        await _context.SaveChangesAsync();
        return fact;
    }

    public async Task<CatFact> SaveCatFactAsync(CatFact fact)
    {
        fact.CreatedAt = DateTime.UtcNow;
        _context.CatFacts.Add(fact);
        await _context.SaveChangesAsync();
        return fact;
    }
    #endregion

    #region 5. NASA APOD API
    public async Task<NasaApod> FetchAndSaveNasaApodAsync(string? date = null)
    {
        var url = string.IsNullOrWhiteSpace(date)
            ? "https://api.nasa.gov/planetary/apod?api_key=DEMO_KEY"
            : $"https://api.nasa.gov/planetary/apod?api_key=DEMO_KEY&date={date}";

        var response = await _httpClient.GetAsync(url);
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();
        using var doc = JsonDocument.Parse(json);
        var root = doc.RootElement;

        var apod = new NasaApod
        {
            Date = root.TryGetProperty("date", out var dProp) ? dProp.GetString() ?? "" : "",
            Title = root.TryGetProperty("title", out var tProp) ? tProp.GetString() ?? "" : "",
            Explanation = root.TryGetProperty("explanation", out var expProp) ? expProp.GetString() : null,
            Url = root.TryGetProperty("url", out var uProp) ? uProp.GetString() : null,
            HdUrl = root.TryGetProperty("hdurl", out var hdProp) ? hdProp.GetString() : null,
            MediaType = root.TryGetProperty("media_type", out var mtProp) ? mtProp.GetString() : null,
            Copyright = root.TryGetProperty("copyright", out var cpProp) ? cpProp.GetString() : null,
            RawJson = json,
            CreatedAt = DateTime.UtcNow
        };

        _context.NasaApods.Add(apod);
        await _context.SaveChangesAsync();
        return apod;
    }

    public async Task<NasaApod> SaveNasaApodAsync(NasaApod apod)
    {
        apod.CreatedAt = DateTime.UtcNow;
        _context.NasaApods.Add(apod);
        await _context.SaveChangesAsync();
        return apod;
    }
    #endregion
}
