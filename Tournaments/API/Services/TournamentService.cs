using API.DTO;
using API.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using ModelsLibrary.Models;
using Org.BouncyCastle.Crypto.Macs;

namespace API.Services
{
    public class TournamentService : ITournamentService
    {
        private readonly ITournamentRepository _tournamentRepository;
        private readonly IMemoryCache _cache;
        public TournamentService(ITournamentRepository tournamentRepository, IMemoryCache memoryCache)
        {
            _tournamentRepository = tournamentRepository;
            _cache = memoryCache;
            
        }
        public async Task<string> CreateTournamentAsync(CreateTournamentDto model)
        {
            // Convert CreateTournamentDto to TournamentModel
            var tournament = new TournamentModel
            {
                Name = model.Name,
                EntryFee = model.EntryFee,
                TeamsLimit = model.TeamsLimit,
                Location = model.Location,
                Description = model.Description,
                TournamentDate = model.TournamentDate,
                OrganizerId = model.OrganizerId,
                StructureType = model.StructureType
            };
            var result = await _tournamentRepository.AddAsync(tournament);

            // Check if the tournament was created successfully
            if (result)
            {
                // Optionally, you can also add the newly created tournament to the cache if needed
                var cacheKey = $"TournamentModel:{tournament.Id}";
                _cache.Set(cacheKey, tournament, TimeSpan.FromMinutes(5));

                // Return the tournament's Id
                return tournament.Id;
            }
            else
            {
                // Return null or an empty string to indicate failure
                return null;
            }
        }

        public async Task<bool> DeleteTournamentByNameAsync(string tournamentName)
        {
            var Id = await _tournamentRepository.DeleteTournamentByNameAsync(tournamentName);
            if (Id != null)
            {
                // If the delete was successful, invalidate the cache for this tournament
                var cacheKey = $"TournamentModel:{Id}";
                _cache.Remove(cacheKey);
                return true;
            }

            return false;
        }
        public async Task<CreateTournamentDto> GetTournamentByNameAsync(string tournamentName)
        {
            var cacheKey = $"TournamentModel:{tournamentName}";

            // Try to get the tournament from the cache
            if (!_cache.TryGetValue(cacheKey, out CreateTournamentDto tournamentDto))
            {
                // If the tournament is not in the cache, fetch it from the database
                tournamentDto = await _tournamentRepository.GetTournamentByNameAsync(tournamentName);

                // Define cache options
                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromMinutes(5)) // Cache expiration
                    .SetPriority(CacheItemPriority.Normal);

                // Save the tournament in the cache
                _cache.Set(cacheKey, tournamentDto, cacheEntryOptions);
            }
            return tournamentDto;
        }
        public async Task<bool> UpdateTournamentStatusByNameAsync(string tournamentName, TournamentStatus newStatus)
        {
            // Call the repository method to update the tournament's status and get the tournament's ID
            var tournamentId = await _tournamentRepository.UpdateTournamentStatusByNameAsync(tournamentName, newStatus);

            if (tournamentId != null)
            {
                // If the update was successful, invalidate the cache for this tournament
                var cacheKey = $"TournamentModel:{tournamentId}";
                _cache.Remove(cacheKey);
                return true;
            }

            return false;
        }

    }
}
