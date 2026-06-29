using RenewableDashboard.Core.Entities;

namespace RenewableDashboard.Core.Interfaces;

public interface IProjectScenarioRepository
{
    Task<IEnumerable<ProjectScenario>> GetAllAsync();
    Task<ProjectScenario?> GetByIdAsync(int id);
    Task<ProjectScenario> AddAsync(ProjectScenario scenario);
    Task DeleteAsync(int id);
}
