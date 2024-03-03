using API.DTO;
using ModelsLibrary.Models;

namespace API.Interfaces;

public interface ITournamentService
{
    Task<string> CreateTournamentAsync(CreateTournamentDto model);
    Task<bool> UpdateTournamentStatusByNameAsync(string tournamentName, TournamentStatus newStatus);
    Task<bool> DeleteTournamentByNameAsync(string tournamentName);
    Task<CreateTournamentDto> GetTournamentByNameAsync(string tournamentName);
}
