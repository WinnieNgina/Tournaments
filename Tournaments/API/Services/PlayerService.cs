using API.DTO;
using API.Interfaces;
using Microsoft.AspNetCore.Identity;
using ModelsLibrary.Models;

namespace API.Services;

public class PlayerService : IPlayerService
{
    private readonly IPlayerRepository _playerRepository;
    private readonly IUserRepository _userRepository;

    public PlayerService(IPlayerRepository playerRepository, IUserRepository userRepository)
    {
        _playerRepository = playerRepository;
        _userRepository = userRepository;
    }

    public async Task<PlayerModel> GetPlayerByIdAsync(string id)
    {
        return await _playerRepository.GetPlayerByIdAsync(id);
    }

    public async Task<PlayerModel> GetPlayerByEmailAsync(string email)
    {
        return await _playerRepository.GetPlayerByEmailAsync(email);
    }

    public async Task<IEnumerable<PlayerDTO>> GetAllPlayersAsync()
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
        return await _userRepository.GenerateEmailConfirmationTokenAsync(playerId);
    }
    public async Task<IdentityResult> ConfirmEmailAsync(string playerId, string token)
    {
        return await _userRepository.ConfirmEmailAsync(playerId, token);
    }
    public async Task<PlayerModel> GetPlayerByUserNameAsync(string userName)
    {
        return await _playerRepository.GetPlayerByUserNameAsync(userName);
    }
    public async Task<bool> CheckPasswordAsync(string playerId, string password)
    {
        return await _userRepository.CheckPasswordAsync(playerId, password);
    }
    public async Task<string> GenerateTwoFactorTokenAsync(string playerId)
    {
        return await _userRepository.GenerateTwoFactorTokenAsync(playerId);
    }
    public async Task<string> GenerateAuthTokenAsync(string playerId)
    {
        return await _userRepository.GenerateAuthTokenAsync(playerId);
    }
    public async Task EnableTwoFactorAuthenticationAsync(string playerId)
    {
        await _userRepository.EnableTwoFactorAuthenticationAsync(playerId);
    }
    public async Task DisableTwoFactorAuthenticationAsync(string playerId)
    {
        await _userRepository.DisableTwoFactorAuthenticationAsync(playerId);
    }
    public async Task LogoutAsync()
    {
        await _userRepository.LogoutAsync();
    }
    public async Task<bool> CheckCurrentPasswordAsync(string playerId, string currentPassword)
    {
        return await _userRepository.CheckCurrentPasswordAsync(playerId, currentPassword);
    }
    public async Task<IdentityResult> ChangePasswordAsync(string playerId, string currentPassword, string newPassword)
    {
        return await _userRepository.ChangePasswordAsync(playerId, currentPassword, newPassword);
    }
    public async Task SignInAsync(string playerId, bool isPersistent)
    {
        await _userRepository.SignInAsync(playerId, isPersistent);
    }
    public async Task<bool> IsPasswordValidAsync(string password)
    {
        return await _userRepository.IsPasswordValidAsync(password);
    }
    public async Task<IdentityResult> AddToRoleAsync(string playerId, string roleName)
    {
        return await _userRepository.AddToRoleAsync(playerId, roleName);
    }

    public async Task<string> GeneratePhoneNumberConfirmationTokenAsync(string Id, string phoneNumber)
    {
        return await _userRepository.GeneratePhoneNumberConfirmationTokenAsync(Id, phoneNumber);
    }

    public async Task<IdentityResult> ConfirmPhoneNumberAsync(string name, string phoneNumber, string token)
    {
        return await _userRepository.ConfirmPhoneNumberAsync(name, phoneNumber, token);
    }
}
