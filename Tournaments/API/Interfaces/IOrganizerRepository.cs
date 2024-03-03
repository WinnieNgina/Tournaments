using API.DTO;
using ModelsLibrary.Models;

namespace API.Interfaces
{
    public interface IOrganizerRepository
    {
        Task<TournamentOrganizerModel> GetOrganizerByIdAsync(string id);
        Task<TournamentOrganizerModel> GetOrganizerByPhoneNumberAsync(string phoneNumber);
        Task<TournamentOrganizerModel> GetOrganizerByEmailAsync(string email);
        Task<TournamentOrganizerModel> GetOrganizerByNameAsync(string userName);
        Task<IEnumerable<OrganizerDTO>> GetAllOrganizersAsync();
        Task<string> CreateOrganizerAsync(SignUpOrganizerDTO model, string password);
        Task<bool> UpdateOrganizerAsync(TournamentOrganizerModel organizer);
        Task<bool> DeleteOrganizerAsync(string id);
    }
}
