using ModelsLibrary.Models;

namespace API.Interfaces;

public interface IPlayerRepository
{
    Task<PlayerModel> GetPlayerByIdAsync(string id);
    Task<PlayerModel> GetPlayerByEmailAsync(string email);
    Task<IEnumerable<PlayerModel>> GetAllPlayersAsync();
    Task CreatePlayerAsync(PlayerModel player, string password);
    Task<bool> UpdatePlayerAsync(PlayerModel player);
    Task<bool> DeletePlayerAsync(string id);
}
