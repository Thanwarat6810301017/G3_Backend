using Project_G3_Group03.Models.Entities;

namespace Project_G3_Group03.Services;

public interface IExternalApiService
{
    // Open Library
    Task<List<OpenLibraryBook>> FetchAndSaveOpenLibraryAsync(string query, int limit = 10);
    Task<OpenLibraryBook> SaveOpenLibraryBookAsync(OpenLibraryBook book);

    // Google Books
    Task<List<GoogleBook>> FetchAndSaveGoogleBooksAsync(string query, int limit = 10);
    Task<GoogleBook> SaveGoogleBookAsync(GoogleBook book);

    // REST Countries
    Task<List<RestCountry>> FetchAndSaveCountriesAsync(string query);
    Task<RestCountry> SaveCountryAsync(RestCountry country);

    // Cat Facts
    Task<CatFact> FetchAndSaveCatFactAsync();
    Task<CatFact> SaveCatFactAsync(CatFact fact);

    // NASA APOD
    Task<NasaApod> FetchAndSaveNasaApodAsync(string? date = null);
    Task<NasaApod> SaveNasaApodAsync(NasaApod apod);
}
