using API.DTO;
using API.Interfaces;
using ModelsLibrary.Models;

namespace API.Services
{
    public class CoachService : ICoachService
    {
        private readonly ICoachRepository _coachRepository;
        private readonly IUserRepository _userRepository;
        public CoachService(ICoachRepository coachRepository, IUserRepository userRepository)
        {
            _coachRepository = coachRepository;
            _userRepository = userRepository;
        }
        public async Task<IEnumerable<CoachDTO>> GetAllCoachesAsync()
        {
            return await _coachRepository.GetAllCoachesAsync();
        }
    }
}
