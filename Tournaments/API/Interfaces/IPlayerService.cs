using API.DTO;
using ModelsLibrary.Models;

namespace API.Interfaces;
public interface IPlayerService
{
    Task<PlayerModelDto> GetPlayerByIdAsync(string id);
    Task<PlayerModelDto> GetPlayerByEmailAsync(string email);
    Task<IEnumerable<PlayerModelDto>> GetAllPlayersAsync();
    Task CreatePlayerAsync(PlayerModel player, string password);
    Task<bool> UpdatePlayerAsync(PlayerModel player);
    Task<bool> DeletePlayerAsync(string id);
}