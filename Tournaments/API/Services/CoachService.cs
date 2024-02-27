using API.DTO;
using API.Interfaces;
using Microsoft.AspNetCore.Identity;
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
        public async Task<string> CreateCoachAsync(CoachModel coach, string password)
        {
            return await _coachRepository.CreateCoachAsync(coach, password);
        }

        public async Task<CoachModel> GetCoachByIdAsync(string id)
        {
            return await _coachRepository.GetCoachByIdAsync(id);
        }

        public async Task<CoachModel> GetCoachByEmailAsync(string email)
        {
            return await _coachRepository.GetCoachByEmailAsync(email);
        }

        public async Task<CoachModel> GetCoachByNameAsync(string userName)
        {
            return await _coachRepository.GetCoachByNameAsync(userName);
        }

        public async Task<bool> IsPasswordValidAsync(string password)
        {
            return await _userRepository.IsPasswordValidAsync(password);
        }

        public Task<IdentityResult> AddToRoleAsync(string Id, string roleName)
        {
            return _userRepository.AddToRoleAsync(Id, roleName);
        }
        public async Task<string> GenerateEmailConfirmationTokenAsync(string Id)
        {
            return await _userRepository.GenerateEmailConfirmationTokenAsync(Id);
        }
        public async Task<IdentityResult> ConfirmEmailAsync(string Id, string token)
        {
            return await _userRepository.ConfirmEmailAsync(Id, token);
        }
        public async Task<bool> CheckPasswordAsync(string Id, string password)
        {
            return await _userRepository.CheckPasswordAsync(Id, password);
        }

        public async Task<string> GenerateTwoFactorTokenAsync(string Id)
        {
            return await _userRepository.GenerateTwoFactorTokenAsync(Id);
        }

        public async Task<string> GenerateAuthTokenAsync(string Id)
        {
            return await _userRepository.GenerateAuthTokenAsync(Id);
        }
    }
}
