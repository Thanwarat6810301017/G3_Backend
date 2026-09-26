using Microsoft.EntityFrameworkCore;
using Project_G3_Group03.Models.Entities;

namespace Project_G3_Group03.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<OpenLibraryBook> OpenLibraryBooks => Set<OpenLibraryBook>();
    public DbSet<GoogleBook> GoogleBooks => Set<GoogleBook>();
    public DbSet<RestCountry> Countries => Set<RestCountry>();
    public DbSet<CatFact> CatFacts => Set<CatFact>();
    public DbSet<NasaApod> NasaApods => Set<NasaApod>();
    public DbSet<XmlRecord> XmlRecords => Set<XmlRecord>();
    public DbSet<JsonRecord> JsonRecords => Set<JsonRecord>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Indexes for OpenLibraryBooks
        modelBuilder.Entity<OpenLibraryBook>(entity =>
        {
            entity.HasIndex(e => e.WorkKey);
            entity.HasIndex(e => e.Title);
            entity.HasIndex(e => e.Author);
        });

        // Indexes for GoogleBooks
        modelBuilder.Entity<GoogleBook>(entity =>
        {
            entity.HasIndex(e => e.GoogleId);
            entity.HasIndex(e => e.Title);
        });

        // Indexes for Countries
        modelBuilder.Entity<RestCountry>(entity =>
        {
            entity.HasIndex(e => e.CommonName);
            entity.HasIndex(e => e.Region);
        });

        // Indexes for CatFacts
        modelBuilder.Entity<CatFact>(entity =>
        {
            entity.HasIndex(e => e.CreatedAt);
        });

        // Indexes for NasaApods
        modelBuilder.Entity<NasaApod>(entity =>
        {
            entity.HasIndex(e => e.Date);
            entity.HasIndex(e => e.Title);
        });

        // Indexes for XmlRecords (Bonus 3: 1,000 Records)
        modelBuilder.Entity<XmlRecord>(entity =>
        {
            entity.HasIndex(e => e.RecordId);
            entity.HasIndex(e => e.Name);
            entity.HasIndex(e => e.Category);
        });

        // Indexes for JsonRecords (Bonus 4: 1,000 Records)
        modelBuilder.Entity<JsonRecord>(entity =>
        {
            entity.HasIndex(e => e.RecordId);
            entity.HasIndex(e => e.Title);
            entity.HasIndex(e => e.Category);
        });
    }
}
