using API.DTO;
using API.Interfaces;
using ModelsLibrary.Models;

namespace API.Services;

public class PlayerService : IPlayerService
{
    private readonly IPlayerRepository _playerRepository;

    public PlayerService(IPlayerRepository playerRepository)
    {
        _playerRepository = playerRepository;
    }

    public async Task<PlayerModelDto> GetPlayerByIdAsync(string id)
    {
        return await _playerRepository.GetPlayerByIdAsync(id);
    }

    public async Task<PlayerModelDto> GetPlayerByEmailAsync(string email)
    {
        return await _playerRepository.GetPlayerByEmailAsync(email);
    }

    public async Task<IEnumerable<PlayerModelDto>> GetAllPlayersAsync()
    {
        return await _playerRepository.GetAllPlayersAsync();
    }

    public async Task CreatePlayerAsync(PlayerModel player, string password)
    {
        await _playerRepository.CreatePlayerAsync(player, password);
    }

    public async Task<bool> UpdatePlayerAsync(PlayerModel player)
    {
        return await _playerRepository.UpdatePlayerAsync(player);
    }

    public async Task<bool> DeletePlayerAsync(string id)
    {
        return await _playerRepository.DeletePlayerAsync(id);
    }
}
