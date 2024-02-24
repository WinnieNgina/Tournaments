using API.Interfaces;
using Microsoft.AspNetCore.Identity;
using ModelsLibrary.Models;

namespace API.Services;

public class PlayerService : IPlayerService
{
    private readonly IPlayerRepository _playerRepository;

    public PlayerService(IPlayerRepository playerRepository)
    {
        _playerRepository = playerRepository;
    }

    public async Task<PlayerModel> GetPlayerByIdAsync(string id)
    {
        return await _playerRepository.GetPlayerByIdAsync(id);
    }

    public async Task<PlayerModel> GetPlayerByEmailAsync(string email)
    {
        return await _playerRepository.GetPlayerByEmailAsync(email);
    }

    public async Task<IEnumerable<PlayerModel>> GetAllPlayersAsync()
    {
        return await _playerRepository.GetAllPlayersAsync();
    }

    public async Task<string> CreatePlayerAsync(PlayerModel player, string password)
    {
        return await _playerRepository.CreatePlayerAsync(player, password);
    }

    public async Task<bool> UpdatePlayerAsync(PlayerModel player)
    {
        return await _playerRepository.UpdatePlayerAsync(player);
    }

    public async Task<bool> DeletePlayerAsync(string id)
    {
        return await _playerRepository.DeletePlayerAsync(id);
    }

    public async Task<string> GenerateEmailConfirmationTokenAsync(string playerId)
    {
        return await _playerRepository.GenerateEmailConfirmationTokenAsync(playerId);
    }
    public async Task<IdentityResult> ConfirmEmailAsync(string playerId, string token)
    {
        return await _playerRepository.ConfirmEmailAsync(playerId, token);
    }
}
