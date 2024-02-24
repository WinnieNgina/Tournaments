using ModelsLibrary.Models;

namespace API.Interfaces;

public interface ICoachRepository
{
    Task<CoachModel> GetCoachByIdAsync(string id);
    Task<CoachModel> GetCoachByEmailAsync(string email);
    Task<IEnumerable<CoachModel>> GetAllCoachesAsync();
    Task CreateCoachAsync(CoachModel player, string password);
    Task<bool> UpdateCoachAsync(CoachModel player);
    Task<bool> DeleteCoachAsync(string id);
}
