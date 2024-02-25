using Microsoft.AspNetCore.Identity;
using ModelsLibrary.Models;

namespace API.Interfaces;

public interface IPlayerRepository
{
    Task<PlayerModel> GetPlayerByIdAsync(string id);
    Task<PlayerModel> GetPlayerByEmailAsync(string email);
    Task<IEnumerable<PlayerModel>> GetAllPlayersAsync();
    Task<string> CreatePlayerAsync(PlayerModel player, string password);
    Task<bool> UpdatePlayerAsync(PlayerModel player);
    Task<bool> DeletePlayerAsync(string id);
    Task<string> GenerateEmailConfirmationTokenAsync(string playerId);
    Task<IdentityResult> ConfirmEmailAsync(string playerId, string token);
    Task<PlayerModel> GetPlayerByUserNameAsync(string userName);
    Task<bool> CheckPasswordAsync(string playerId, string password);
    Task<string> GenerateTwoFactorTokenAsync(string playerId);
    Task<string> GenerateAuthTokenAsync(string playerId);
    Task EnableTwoFactorAuthenticationAsync(string playerId);
    Task DisableTwoFactorAuthenticationAsync(string playerId);
    Task LogoutAsync();
    Task<bool> CheckCurrentPasswordAsync(string playerId, string currentPassword);
    Task<IdentityResult> ChangePasswordAsync(string playerId, string currentPassword, string newPassword);
    Task SignInAsync(string playerId, bool isPersistent);
}
