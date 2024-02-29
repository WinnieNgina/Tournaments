using API.DTO;
using ModelsLibrary.Models;

namespace API.Interfaces;

public interface ICoachRepository
{
    Task<CoachModel> GetCoachByIdAsync(string id);
    Task<CoachModel> GetCoachByEmailAsync(string email);
    Task<CoachModel> GetCoachByNameAsync(string userName);
    Task<IEnumerable<CoachDTO>> GetAllCoachesAsync();
    Task<string> CreateCoachAsync(CoachModel coach, string password);
    Task<bool> UpdateCoachAsync(CoachModel coach);
    Task<bool> DeleteCoachAsync(string id);
}
