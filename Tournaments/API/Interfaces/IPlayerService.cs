﻿using API.DTO;
using Microsoft.AspNetCore.Identity;
using ModelsLibrary.Models;

namespace API.Interfaces;
public interface IPlayerService
{
    Task<PlayerModel> GetPlayerByIdAsync(string id);
    Task<PlayerModel> GetPlayerByEmailAsync(string email);
    Task<IEnumerable<PlayerDTO>> GetAllPlayersAsync();
    Task<string> CreatePlayerAsync(CreatePlayerDto model, string password);
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
    Task<bool> IsPasswordValidAsync(string password);
    Task<IdentityResult> AddToRoleAsync(string playerId, string roleName);
    Task<string> GeneratePhoneNumberConfirmationTokenAsync(string Id, string phoneNumber);
    Task<IdentityResult> ConfirmPhoneNumberAsync(string name, string phoneNumber, string token);
    Task<PlayerModel> GetPlayerByPhoneNumberAsync(string phoneNumber);
}