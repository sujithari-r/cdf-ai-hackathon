using Microsoft.EntityFrameworkCore;
using RenewableDashboard.Core.Entities;
using RenewableDashboard.Core.Interfaces;
using RenewableDashboard.Infrastructure.Data;

namespace RenewableDashboard.Infrastructure.Repositories;

public class ProjectScenarioRepository : IProjectScenarioRepository
{
    private readonly AppDbContext _context;

    public ProjectScenarioRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<ProjectScenario>> GetAllAsync() =>
        await _context.ProjectScenarios.OrderByDescending(s => s.CreatedAt).ToListAsync();

    public async Task<ProjectScenario?> GetByIdAsync(int id) =>
        await _context.ProjectScenarios.FindAsync(id);

    public async Task<ProjectScenario> AddAsync(ProjectScenario scenario)
    {
        _context.ProjectScenarios.Add(scenario);
        await _context.SaveChangesAsync();
        return scenario;
    }

    public async Task DeleteAsync(int id)
    {
        var scenario = await _context.ProjectScenarios.FindAsync(id);
        if (scenario != null)
        {
            _context.ProjectScenarios.Remove(scenario);
            await _context.SaveChangesAsync();
        }
    }
}
