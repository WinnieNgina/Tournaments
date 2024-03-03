using API.DTO;
using Microsoft.AspNetCore.Identity;
using ModelsLibrary.Models;

namespace API.Interfaces;

public interface ICoachService
{
    Task<IEnumerable<CoachDTO>> GetAllCoachesAsync();
    Task<string> CreateCoachAsync(SignUpCoachDTO model, string password);
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
    Task EnableTwoFactorAuthenticationAsync(string Id);
    Task DisableTwoFactorAuthenticationAsync(string Id);
    Task LogoutAsync();
    Task<bool> DeleteCoachAsync(string id);
    Task<bool> CheckCurrentPasswordAsync(string Id, string currentPassword);
    Task<IdentityResult> ChangePasswordAsync(string Id, string currentPassword, string newPassword);
    Task SignInAsync(string Id, bool isPersistent);
    Task<string> GeneratePhoneNumberConfirmationTokenAsync(string Id, string phoneNumber);
    Task<IdentityResult> ConfirmPhoneNumberAsync(string name, string phoneNumber, string token);
    Task<CoachModel> GetCoachByPhoneNumberAsync(string phoneNumber);
}
