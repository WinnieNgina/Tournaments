using Microsoft.AspNetCore.Identity;
using ModelsLibrary.Models;

namespace API.Interfaces;
public interface IPlayerService
{
    Task<PlayerModel> GetPlayerByIdAsync(string id);
    Task<PlayerModel> GetPlayerByEmailAsync(string email);
    Task<IEnumerable<PlayerModel>> GetAllPlayersAsync();
    Task<string> CreatePlayerAsync(PlayerModel player, string password);
    Task<bool> UpdatePlayerAsync(PlayerModel player);
    Task<bool> DeletePlayerAsync(string id);
    Task<string> GenerateEmailConfirmationTokenAsync(string playerId);
    Task<IdentityResult> ConfirmEmailAsync(string playerId, string token);
}