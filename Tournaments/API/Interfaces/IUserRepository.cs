using Microsoft.AspNetCore.Identity;

namespace API.Interfaces;

public interface IUserRepository
{
    Task SignInAsync(string userId, bool isPersistent);
    Task<string> GenerateEmailConfirmationTokenAsync(string userId);
    Task<IdentityResult> ConfirmEmailAsync(string userId, string token);
    Task<bool> CheckPasswordAsync(string userId, string password);
    Task<string> GenerateTwoFactorTokenAsync(string userId);
    Task<string> GenerateAuthTokenAsync(string userId);
    Task EnableTwoFactorAuthenticationAsync(string userId);
    Task DisableTwoFactorAuthenticationAsync(string userId);
    Task LogoutAsync();
    Task<bool> CheckCurrentPasswordAsync(string userId, string currentPassword);
    Task<IdentityResult> ChangePasswordAsync(string userId, string currentPassword, string newPassword);
    Task<bool> IsPasswordValidAsync(string password);
    Task<IdentityResult> AddToRoleAsync(string userId, string roleName);
    Task<string> GeneratePhoneNumberConfirmationTokenAsync(string userId, string phoneNumber);
    Task<IdentityResult> ConfirmPhoneNumberAsync(string name, string phoneNumber, string token);
}
