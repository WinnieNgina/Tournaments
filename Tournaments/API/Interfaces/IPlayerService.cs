using API.DTO;
using ModelsLibrary.Models;

namespace API.Interfaces;
public interface IPlayerService
{
    Task<PlayerModel> GetPlayerByIdAsync(string id);
    Task<PlayerModel> GetPlayerByEmailAsync(string email);
    Task<IEnumerable<PlayerModel>> GetAllPlayersAsync();
    Task<bool> CreatePlayerAsync(PlayerModel player, string password);
    Task<bool> UpdatePlayerAsync(PlayerModel player);
    Task<bool> DeletePlayerAsync(string id);
    Task<string> GenerateEmailConfirmationTokenAsync(PlayerModel playerModel);
}