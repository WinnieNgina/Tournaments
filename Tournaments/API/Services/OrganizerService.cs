using API.DTO;
using API.Interfaces;
using Microsoft.AspNetCore.Identity;
using ModelsLibrary.Models;

namespace API.Services
{
    public class OrganizerService : IOrganizerService
    {
        private readonly IOrganizerRepository _organizerRepository;
        private readonly IUserRepository _userRepository;

        public OrganizerService(IOrganizerRepository organizerRepository, IUserRepository userRepository)
        {
            _organizerRepository = organizerRepository;
            _userRepository = userRepository;
        }

        public async Task<IdentityResult> AddToRoleAsync(string Id, string roleName)
        {
           return await _userRepository.AddToRoleAsync(Id, roleName);
        }

        public async Task<IdentityResult> ChangePasswordAsync(string Id, string currentPassword, string newPassword)
        {
            return await _userRepository.ChangePasswordAsync(Id, currentPassword, newPassword);
        }

        public async Task<bool> CheckCurrentPasswordAsync(string Id, string currentPassword)
        {
            return await _userRepository.CheckCurrentPasswordAsync(Id, currentPassword);
        }

        public async Task<bool> CheckPasswordAsync(string Id, string password)
        {
            return await _userRepository.CheckPasswordAsync(Id, password);
        }

        public async Task<IdentityResult> ConfirmEmailAsync(string Id, string token)
        {
            return await _userRepository.ConfirmEmailAsync(Id, token);
        }

        public async Task<IdentityResult> ConfirmPhoneNumberAsync(string name, string phoneNumber, string token)
        {
            return await _userRepository.ConfirmPhoneNumberAsync(name, phoneNumber, token);
        }

        public async Task<string> CreateOrganizerAsync(TournamentOrganizerModel orgnazier, string password)
        {
            return await _organizerRepository.CreateOrganizerAsync(orgnazier, password);
        }

        public async Task<bool> DeleteOrganizerAsync(string id)
        {
            return await _organizerRepository.DeleteOrganizerAsync(id);
        }

        public async Task DisableTwoFactorAuthenticationAsync(string Id)
        {
            await _userRepository.DisableTwoFactorAuthenticationAsync(Id);
        }

        public async Task EnableTwoFactorAuthenticationAsync(string Id)
        {
            await _userRepository.EnableTwoFactorAuthenticationAsync(Id);
        }

        public async Task<string> GenerateAuthTokenAsync(string Id)
        {
            return await _userRepository.GenerateAuthTokenAsync(Id);
        }

        public async Task<string> GenerateEmailConfirmationTokenAsync(string Id)
        {
            return await _userRepository.GenerateEmailConfirmationTokenAsync(Id);
        }

        public async Task<string> GeneratePhoneNumberConfirmationTokenAsync(string userId, string phoneNumber)
        {
            return await _userRepository.GeneratePhoneNumberConfirmationTokenAsync(userId, phoneNumber);
        }

        public async Task<string> GenerateTwoFactorTokenAsync(string Id)
        {
            return await _userRepository.GenerateTwoFactorTokenAsync(Id);
        }

        public async Task<IEnumerable<OrganizerDTO>> GetAllOrganizersAsync()
        {
            return await _organizerRepository.GetAllOrganizersAsync();
        }

        public async Task<TournamentOrganizerModel> GetOrganizerByEmailAsync(string email)
        {
            return await _organizerRepository.GetOrganizerByEmailAsync(email);
        }

        public async Task<TournamentOrganizerModel> GetOrganizerByIdAsync(string id)
        {
            return await _organizerRepository.GetOrganizerByIdAsync(id);
        }

        public async Task<TournamentOrganizerModel> GetOrganizerByNameAsync(string userName)
        {
            return await _organizerRepository.GetOrganizerByNameAsync(userName);
        }

        public async Task<TournamentOrganizerModel> GetOrganizerByPhoneNumberAsync(string phoneNumber)
        {
            return await _organizerRepository.GetOrganizerByPhoneNumberAsync(phoneNumber);
        }

        public async Task<bool> IsPasswordValidAsync(string password)
        {
            return await _userRepository.IsPasswordValidAsync(password);
        }

        public async Task LogoutAsync()
        {
            await _userRepository.LogoutAsync();
        }

        public async Task SignInAsync(string Id, bool isPersistent)
        {
            _userRepository.SignInAsync(Id, isPersistent);
        }

        public async Task<bool> UpdateOrganizerAsync(TournamentOrganizerModel organizer)
        {
            return await _organizerRepository.UpdateOrganizerAsync(organizer);
        }
    }
}
