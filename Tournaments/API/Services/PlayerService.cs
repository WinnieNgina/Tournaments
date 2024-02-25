using API.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.OpenApi.Writers;
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
    public async Task<PlayerModel> GetPlayerByUserNameAsync(string userName)
    {
        return await _playerRepository.GetPlayerByUserNameAsync(userName);
    }
    public async Task<bool> CheckPasswordAsync(string playerId, string password)
    {
        return await _playerRepository.CheckPasswordAsync(playerId, password);
    }
    public async Task<string> GenerateTwoFactorTokenAsync(string playerId)
    {
        return await _playerRepository.GenerateTwoFactorTokenAsync(playerId);
    }
    public async Task<string> GenerateAuthTokenAsync(string playerId)
    {
        return await _playerRepository.GenerateAuthTokenAsync(playerId);
    }
    public async Task EnableTwoFactorAuthenticationAsync(string playerId)
    {
        await _playerRepository.EnableTwoFactorAuthenticationAsync(playerId) ;
    }
    public async Task DisableTwoFactorAuthenticationAsync(string playerId)
    {
        await _playerRepository.DisableTwoFactorAuthenticationAsync(playerId) ;
    }
    public async Task LogoutAsync()
    {
        await _playerRepository.LogoutAsync() ;
    }
    public async Task<bool> CheckCurrentPasswordAsync(string playerId, string currentPassword)
    {
        return await _playerRepository.CheckCurrentPasswordAsync(playerId, currentPassword);
    }
    public async Task<IdentityResult> ChangePasswordAsync(string playerId, string currentPassword, string newPassword)
    {
        return await _playerRepository.ChangePasswordAsync(playerId, currentPassword, newPassword);
    }
    public async Task SignInAsync(string playerId, bool isPersistent)
    {
        await _playerRepository.SignInAsync(playerId, isPersistent);
    }
}
