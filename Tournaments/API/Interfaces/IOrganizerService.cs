using API.DTO;
using Microsoft.AspNetCore.Identity;
using ModelsLibrary.Models;

namespace API.Interfaces
{
    public interface IOrganizerService
    {
        Task<TournamentOrganizerModel> GetOrganizerByIdAsync(string id);
        Task<TournamentOrganizerModel> GetOrganizerByEmailAsync(string email);
        Task<TournamentOrganizerModel> GetOrganizerByNameAsync(string userName);
        Task<IEnumerable<OrganizerDTO>> GetAllOrganizersAsync();
        Task<string> CreateOrganizerAsync(SignUpOrganizerDTO model, string password);
        Task<bool> UpdateOrganizerAsync(TournamentOrganizerModel organizer);
        Task<bool> DeleteOrganizerAsync(string id);
        Task<bool> IsPasswordValidAsync(string password);
        Task<IdentityResult> AddToRoleAsync(string Id, string roleName);
        Task<string> GenerateEmailConfirmationTokenAsync(string Id);
        Task<IdentityResult> ConfirmEmailAsync(string Id, string token);
        Task<bool> CheckPasswordAsync(string Id, string password);
        Task<string> GenerateTwoFactorTokenAsync(string Id);
        Task<string> GenerateAuthTokenAsync(string Id);
        Task EnableTwoFactorAuthenticationAsync(string Id);
        Task DisableTwoFactorAuthenticationAsync(string Id);
        Task LogoutAsync();
        Task<bool> CheckCurrentPasswordAsync(string Id, string currentPassword);
        Task<IdentityResult> ChangePasswordAsync(string Id, string currentPassword, string newPassword);
        Task SignInAsync(string Id, bool isPersistent);
        Task<TournamentOrganizerModel> GetOrganizerByPhoneNumberAsync(string phoneNumber);
        Task<string> GeneratePhoneNumberConfirmationTokenAsync(string userId, string phoneNumber);
        Task<IdentityResult> ConfirmPhoneNumberAsync(string name, string phoneNumber, string token);
    }
}
