using API.DTO;
using ModelsLibrary.Models;

namespace API.Interfaces;

public interface ITournamentRepository
{
    Task<bool> AddAsync(TournamentModel tournament);
    Task<string> UpdateTournamentStatusByNameAsync(string tournamentName, TournamentStatus newStatus);
    Task<string> DeleteTournamentByNameAsync(string tournamentName);
    Task<CreateTournamentDto> GetTournamentByNameAsync(string tournamentName);
}
