using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace RenewableDashboard.Data;

/// <summary>
/// Design-time factory so `dotnet ef` can create migrations without booting the app.
/// </summary>
public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
        optionsBuilder.UseSqlite("Data Source=renewable.db");
        return new AppDbContext(optionsBuilder.Options);
    }
}
