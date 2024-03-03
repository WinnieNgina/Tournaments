using API.DTO;
using API.Interfaces;
using Microsoft.EntityFrameworkCore;
using ModelsLibrary.DataAccess;
using ModelsLibrary.Models;

namespace API.Repository;

public class TournamentRepository : ITournamentRepository
{
    private readonly DataContext _context;

    public TournamentRepository(DataContext context)
    {
        _context = context;
    }

    public async Task<bool> AddAsync(TournamentModel tournament)
    {
        try
        {
            // Add the tournament to the DbSet
            _context.Tournament.Add(tournament);

            // Save the changes to the database
            await _context.SaveChangesAsync();

            return true;
        }
        catch
        { 
            return false;
        }
    }
    public async Task<string> UpdateTournamentStatusByNameAsync(string tournamentName, TournamentStatus newStatus)
    {
        var tournament = _context.Tournament.First(t => t.Name == tournamentName);

        if (tournament == null)
        {
            return null; // Tournament not found
        }

        tournament.Status = newStatus;
        _context.Tournament.Update(tournament);
        await _context.SaveChangesAsync();

        return tournament.Id; // Return the tournament's ID
    }
    public async Task<string> DeleteTournamentByNameAsync(string tournamentName)
    {
        var tournament = _context.Tournament.First(t => t.Name == tournamentName);

        if (tournament == null)
        {
            return null; // Tournament not found
        }
        _context.Tournament.Remove(tournament);
        await _context.SaveChangesAsync();

        return tournament.Id; // Return the tournament's ID
    }
    public async Task<CreateTournamentDto> GetTournamentByNameAsync(string tournamentName)
    {
        var tournamentDto = await _context.Tournament
            .Where(t => t.Name == tournamentName)
            .Select(t => new CreateTournamentDto
            {
                Name = t.Name,
                EntryFee = t.EntryFee,
                TeamsLimit = t.TeamsLimit,
                Location = t.Location,
                Description = t.Description,
                TournamentDate = t.TournamentDate,
                OrganizerId = t.OrganizerId,
                StructureType = t.StructureType,
            })
            .FirstOrDefaultAsync();
        if (tournamentDto == null)
        {
            return null; // Tournament not found
        }

        return tournamentDto;
    }
}


