using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace MagicWise.Data;

/// <summary>
/// Used only by EF Core design-time tools (dotnet-ef migrations add, etc.).
/// Not used at runtime.
/// </summary>
public class MagicWiseDbContextFactory : IDesignTimeDbContextFactory<MagicWiseDbContext>
{
    public MagicWiseDbContext CreateDbContext(string[] args)
    {
        var options = new DbContextOptionsBuilder<MagicWiseDbContext>()
            .UseSqlite("Data Source=design-time.db")
            .Options;

        return new MagicWiseDbContext(options);
    }
}
