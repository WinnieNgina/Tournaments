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
    Task<PlayerModel> GetPlayerByUserNameAsync(string userName);
}
