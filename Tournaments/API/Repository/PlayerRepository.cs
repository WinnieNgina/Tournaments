using Microsoft.Extensions.Caching.Memory;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using API.Interfaces;
using ModelsLibrary.Models;
using ModelsLibrary.DataAccess;
using API.DTO;

public class PlayerRepository : IPlayerRepository
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly DataContext _context;
    private readonly IMemoryCache _cache;
    private IQueryable<PlayerModel> Players => _userManager.Users.OfType<PlayerModel>();


    public PlayerRepository(UserManager<User> userManager, SignInManager<User> signInManager, DataContext context, IMemoryCache cache)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _context = context;
        _cache = cache;
    }

    public async Task<PlayerModelDto> GetPlayerByIdAsync(string id)
    {
        // Define a cache key based on the player ID
        var cacheKey = $"PlayerModel:{id}";

        // Try to get the player from the cache
        if (!_cache.TryGetValue(cacheKey, out PlayerModelDto player))
        {
            // If the player is not in the cache, fetch it from the database
            var user = await Players
                .Where(u => u.Id == id) // Further filtering by Id
                .Select(u => new PlayerModelDto
                {
                    Id = u.Id,
                    UserName = u.UserName,
                    Email = u.Email,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    AreaOfResidence = u.AreaOfResidence,
                    DateOfBirth = u.DateOfBirth,
                    NextOfKinCOntactInfo = u.NextOfKin.ContactInfo,
                    Status = u.Status,
                    TeamNames = u.Teams.Select(t => t.Team.Name).ToList()
                })
                .FirstAsync() ;
            if (user != null)
            {
                player = user;

                // Define cache options
                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromMinutes(5)) // Cache expiration
                    .SetPriority(CacheItemPriority.Normal);

                // Save the player in the cache
                _cache.Set(cacheKey, player, cacheEntryOptions);
            }
        }

        return player;
    }
    public async Task<PlayerModelDto> GetPlayerByEmailAsync(string email)
    {
        // Define a cache key based on the player email
        var cacheKey = $"PlayerModel:{email}";

        // Try to get the player from the cache
        if (!_cache.TryGetValue(cacheKey, out PlayerModelDto player))
        {
            // If the player is not in the cache, fetch it from the database
            var user = await Players
                .Where(u => u.Email == email) // Further filtering by Id
                .Select(u => new PlayerModelDto
                {
                    Id = u.Id,
                    UserName = u.UserName,
                    Email = u.Email,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    AreaOfResidence = u.AreaOfResidence,
                    DateOfBirth = u.DateOfBirth,
                    NextOfKinCOntactInfo = u.NextOfKin.ContactInfo,
                    Status = u.Status,
                    TeamNames = u.Teams.Select(t => t.Team.Name).ToList()
                })
                .FirstAsync();
            if (user != null)
            {
                player = user;

                // Define cache options
                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromMinutes(5)) // Cache expiration
                    .SetPriority(CacheItemPriority.Normal);

                // Save the player in the cache
                _cache.Set(cacheKey, player, cacheEntryOptions);
            }
        }

        return player;
    }
    public async Task<IEnumerable<PlayerModelDto>> GetAllPlayersAsync()
    {
        // Define a cache key for all players
        var cacheKey = "AllPlayers";

        // Try to get the players from the cache
        if (!_cache.TryGetValue(cacheKey, out IEnumerable<PlayerModelDto> players))
        {
            // If the players are not in the cache, fetch them from the database using the Players property
            var users = await Players
                .Select(p => new PlayerModelDto
                {
                    Id = p.Id,
                    UserName = p.UserName,
                    Email = p.Email,
                    FirstName = p.FirstName,
                    LastName = p.LastName,
                    AreaOfResidence = p.AreaOfResidence,
                    DateOfBirth = p.DateOfBirth,
                    NextOfKinCOntactInfo = p.NextOfKin.ContactInfo,
                    Status = p.Status,
                    TeamNames = p.Teams.Select(t => t.Team.Name).ToList() // Assuming TeamModel has a Name property
                                                                          // Map other properties as needed
                })
                .ToListAsync();

            // Cast the results to PlayerModel
            players = users.Cast<PlayerModelDto>();

            // Define cache options
            var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromMinutes(5)) // Cache expiration
                .SetPriority(CacheItemPriority.High);

            // Save the players in the cache
            _cache.Set(cacheKey, players, cacheEntryOptions);
        }

        return players;
    }



    public async Task CreatePlayerAsync(PlayerModel player, string password)
    {
        // Convert PlayerModel to User or ApplicationIdentityUser based on your setup
        var user = new PlayerModel 
        {
            FirstName = player.FirstName,
            LastName = player.LastName,
            AreaOfResidence = player.AreaOfResidence,
            UserName = player.UserName,
            Email = player.Email,
            UserType = "Player" // Assuming UserType is a property in your User class
        };

        // Use UserManager to create the user
        var result = await _userManager.CreateAsync(user, password);

        // Check if the user was created successfully
        if (result.Succeeded)
        {
            // Remove the cached list of all players
            _cache.Remove("AllPlayerModels");

            // Optionally, you can also add the newly created player to the cache if needed
            // This is not necessary for invalidating the cache but can be useful for reducing database calls
            var cacheKey = $"PlayerModel:{user.Id}";
            _cache.Set(cacheKey, player, TimeSpan.FromMinutes(5));
        }
        else
        {
            // Handle the failure (e.g., throw an exception or return an error)
            throw new Exception("Failed to add player");
        }
    }

    public async Task<bool> UpdatePlayerAsync(PlayerModel player)
    {
        // Check if the player exists and is of UserType "Player"
        var existingPlayer = await Players.FirstAsync(p => p.Id == player.Id);
        if (existingPlayer != null)
        {

            // Update player in the database
            var result = await _userManager.UpdateAsync(player);
            // Clear the cache for the specific player
            _cache.Remove($"PlayerModel:{existingPlayer.Id}");
            // Clear the cache for all players
            _cache.Remove("AllPlayers");
            return result.Succeeded;
        }

        return false; // Player does not exist or is not of UserType "Player"
    }


    public async Task<bool> DeletePlayerAsync(string id)
    {
        // Use the Players property to find the player
        var user = await Players.FirstAsync(p => p.Id == id);
        if (user != null)
        {
            var result = await _userManager.DeleteAsync(user);
            _cache.Remove($"PlayerModel:{id}"); // Clear the cache for the specific player
            _cache.Remove("AllPlayers"); // Clear the cache for all players
            return result.Succeeded; // Returns true if deletion was successful
        }
        return false; // User does not exist
    }

}
