using API.DTO;
using ModelsLibrary.Models;

namespace API.Interfaces;

public interface ICoachService
{
    Task<IEnumerable<CoachDTO>> GetAllCoachesAsync();
}
