using API.DTO;
using Microsoft.AspNetCore.Identity;
using ModelsLibrary.Models;

namespace API.Interfaces;

public interface ICoachService
{
    Task<IEnumerable<CoachDTO>> GetAllCoachesAsync();
    Task<string> CreateCoachAsync(CoachModel coach, string password);
    Task<CoachModel> GetCoachByIdAsync(string id);
    Task<CoachModel> GetCoachByEmailAsync(string email);
    Task<CoachModel> GetCoachByNameAsync(string userName);
    Task<bool> IsPasswordValidAsync(string password);
    Task<IdentityResult> AddToRoleAsync(string Id, string roleName);
    Task<string> GenerateEmailConfirmationTokenAsync(string Id);
    Task<IdentityResult> ConfirmEmailAsync(string Id, string token);
    Task<bool> CheckPasswordAsync(string Id, string password);
    Task<string> GenerateTwoFactorTokenAsync(string Id);
    Task<string> GenerateAuthTokenAsync(string Id);
}
