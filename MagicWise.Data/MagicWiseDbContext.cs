using MagicWise.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace MagicWise.Data;

public class MagicWiseDbContext : DbContext
{
    public MagicWiseDbContext(DbContextOptions<MagicWiseDbContext> options) : base(options)
    {
    }

    public DbSet<TagCategory> TagCategories { get; set; } = null!;
    public DbSet<Tag> Tags { get; set; } = null!;
    public DbSet<DestinationTag> DestinationTags { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<TagCategory>(entity =>
        {
            entity.HasIndex(e => e.Name).IsUnique();
        });

        modelBuilder.Entity<Tag>(entity =>
        {
            entity.HasIndex(e => new { e.CategoryId, e.Name }).IsUnique();
            entity.HasOne(e => e.Category)
                  .WithMany(c => c.Tags)
                  .HasForeignKey(e => e.CategoryId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<DestinationTag>(entity =>
        {
            entity.HasIndex(e => new { e.DestinationId, e.TagId }).IsUnique();
            entity.HasOne(e => e.Tag)
                  .WithMany(t => t.DestinationTags)
                  .HasForeignKey(e => e.TagId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        SeedData(modelBuilder);
    }

    private static void SeedData(ModelBuilder modelBuilder)
    {
        // --- Tag Categories ---
        modelBuilder.Entity<TagCategory>().HasData(
            new TagCategory { Id = 1, Name = "Company",  DisplayOrder = 1, IsVisible = true },
            new TagCategory { Id = 2, Name = "Region",   DisplayOrder = 2, IsVisible = true },
            new TagCategory { Id = 3, Name = "Country",  DisplayOrder = 3, IsVisible = true }
        );

        // --- Company Tags ---
        modelBuilder.Entity<Tag>().HasData(
            new Tag { Id = 1,  Name = "Disney",                         CategoryId = 1 },
            new Tag { Id = 2,  Name = "Universal",                      CategoryId = 1 },
            new Tag { Id = 3,  Name = "Six Flags",                      CategoryId = 1 },
            new Tag { Id = 4,  Name = "SeaWorld Parks & Entertainment", CategoryId = 1 },
            new Tag { Id = 5,  Name = "Merlin Entertainments",          CategoryId = 1 },
            new Tag { Id = 6,  Name = "Cedar Fair / Paramount",         CategoryId = 1 },
            new Tag { Id = 7,  Name = "Herschend Family Entertainment", CategoryId = 1 },
            new Tag { Id = 8,  Name = "Parques Reunidos",               CategoryId = 1 },
            new Tag { Id = 9,  Name = "Efteling",                       CategoryId = 1 },
            new Tag { Id = 10, Name = "Europa-Park",                    CategoryId = 1 }
        );

        // --- Region Tags ---
        modelBuilder.Entity<Tag>().HasData(
            new Tag { Id = 11, Name = "North America",     CategoryId = 2 },
            new Tag { Id = 12, Name = "Europe",            CategoryId = 2 },
            new Tag { Id = 13, Name = "Asia-Pacific",      CategoryId = 2 },
            new Tag { Id = 14, Name = "Middle East",       CategoryId = 2 },
            new Tag { Id = 15, Name = "Latin America",     CategoryId = 2 }
        );

        // --- Country Tags ---
        modelBuilder.Entity<Tag>().HasData(
            new Tag { Id = 16, Name = "United States",    CategoryId = 3 },
            new Tag { Id = 17, Name = "United Kingdom",   CategoryId = 3 },
            new Tag { Id = 18, Name = "France",           CategoryId = 3 },
            new Tag { Id = 19, Name = "Japan",            CategoryId = 3 },
            new Tag { Id = 20, Name = "China",            CategoryId = 3 },
            new Tag { Id = 21, Name = "Canada",           CategoryId = 3 },
            new Tag { Id = 22, Name = "Australia",        CategoryId = 3 },
            new Tag { Id = 23, Name = "United Arab Emirates", CategoryId = 3 },
            new Tag { Id = 24, Name = "Netherlands",      CategoryId = 3 },
            new Tag { Id = 25, Name = "Germany",          CategoryId = 3 },
            new Tag { Id = 26, Name = "Spain",            CategoryId = 3 },
            new Tag { Id = 27, Name = "Belgium",          CategoryId = 3 },
            new Tag { Id = 28, Name = "Denmark",          CategoryId = 3 },
            new Tag { Id = 29, Name = "Sweden",           CategoryId = 3 },
            new Tag { Id = 30, Name = "Hong Kong",        CategoryId = 3 }
        );

        // --- Destination → Tag mappings ---
        // Slugs/IDs from the themeparks.wiki API (destination slugs are used as DestinationId)
        int id = 1;
        void Map(string destinationId, params int[] tagIds)
        {
            foreach (int tagId in tagIds)
            {
                modelBuilder.Entity<DestinationTag>().HasData(
                    new DestinationTag { Id = id++, DestinationId = destinationId, TagId = tagId }
                );
            }
        }

        // Walt Disney World Resort (US)
        Map("waltdisneyworldresort",    1, 11, 16);
        // Disneyland Resort (US)
        Map("disneylandresort",         1, 11, 16);
        // Disneyland Paris
        Map("dlp",                      1, 12, 18);
        // Tokyo Disney Resort
        Map("tdr",                      1, 13, 19);
        // Hong Kong Disneyland
        Map("hongkongdisneylandpark",   1, 13, 30);
        // Shanghai Disney Resort
        Map("shanghaidisneyresort",     1, 13, 20);
        // Universal Orlando Resort
        Map("universalresort_orlando",  2, 11, 16);
        // Universal Studios Hollywood
        Map("universalresort_hollywood", 2, 11, 16);
        // Universal Studios Japan
        Map("universalstudiosjapan",    2, 13, 19);
        // Universal Studios Singapore
        Map("universalsingapore",       2, 13);
        // Six Flags Magic Mountain
        Map("sixflags_destination_SFMM", 3, 11, 16);
        // Six Flags Great Adventure
        Map("sixflags_destination_GADV", 3, 11, 16);
        // Six Flags Over Georgia
        Map("sixflags_destination_SFOG", 3, 11, 16);
        // Six Flags Over Texas
        Map("sixflags_destination_SFOT", 3, 11, 16);
        // Six Flags Great America
        Map("sixflags_destination_SFGR", 3, 11, 16);
        // SeaWorld Orlando
        Map("seaworldorlandoresort",    4, 11, 16);
        // SeaWorld San Diego
        Map("seaworldsandiego",         4, 11, 16);
        // SeaWorld San Antonio
        Map("seaworldsanantonio",       4, 11, 16);
        // Busch Gardens Tampa
        Map("buschgardenstampa",        4, 11, 16);
        // Busch Gardens Williamsburg
        Map("buschgardenswillamsburg",  4, 11, 16);
        // LEGOLAND California
        Map("legolandcaliforniaresort", 5, 11, 16);
        // LEGOLAND Florida
        Map("legolandorlandoresort",    5, 11, 16);
        // LEGOLAND Windsor
        Map("legolandwindsorresort",    5, 12, 17);
        // LEGOLAND Deutschland
        Map("legolanddeutschlandresort", 5, 12, 25);
        // Alton Towers
        Map("altontowersresort",        5, 12, 17);
        // Thorpe Park
        Map("thorpeparkresort",         5, 12, 17);
        // Chessington World of Adventures
        Map("chessingtonworldofadventuresresort", 5, 12, 17);
        // Gardaland
        Map("gardalandresort",          5, 12);
        // Efteling
        Map("eftelingresort",           9, 12, 24);
        // Europa-Park
        Map("europapark",               10, 12, 25);
        // Phantasialand
        Map("phantasialanddest",        12, 25);
        // Tivoli Gardens (not currently tracked by the themeparks.wiki API;
        // left in place in case it's added later)
        Map("tivoligardens",            12, 28);
        // Liseberg
        Map("liseberg",                 12, 29);
        // PortAventura World
        Map("portaventuraworld",        8, 12, 26);
        // Plopsaland (themeparks.wiki tracks the Belgium and Deutschland parks
        // as two separate destinations under the same brand)
        Map("plopsalanddeutschland",    8, 12, 27);
        Map("plopsaland-de-panne",      8, 12, 27);
        // IMG Worlds of Adventure (not currently tracked by the themeparks.wiki
        // API; left in place in case it's added later)
        Map("imgworldsofadventure",     14, 23);
        // Ferrari World Abu Dhabi (not currently tracked by the themeparks.wiki
        // API; left in place in case it's added later)
        Map("ferrariworldabudhabi",     14, 23);
        // Warner Bros. Movie World Abu Dhabi
        Map("vrtp_mw_te2_destination",  14, 23);
    }
}
