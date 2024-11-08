﻿using API.DTO;
using API.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using ModelsLibrary.DataAccess;
using ModelsLibrary.Models;
namespace API.Repository;
public class PlayerRepository : IPlayerRepository
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly DataContext _context;
    private readonly IMemoryCache _cache;
    private readonly IConfiguration _configuration;

    public PlayerRepository(UserManager<User> userManager, SignInManager<User> signInManager, DataContext context, IMemoryCache cache, IConfiguration configuration)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _context = context;
        _cache = cache;
        _configuration = configuration;
    }
    public async Task<PlayerModel> GetPlayerByIdAsync(string id)
    {
        // Define a cache key based on the player ID
        var cacheKey = $"PlayerModel:{id}";

        // Try to get the player from the cache
        if (!_cache.TryGetValue(cacheKey, out PlayerModel player))
        {
            // If the player is not in the cache, fetch it from the database
            var user = await _userManager.FindByIdAsync(id);
            if (user != null && user.UserType == "Player")
            {
                player = (PlayerModel)user;

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
    public async Task<PlayerModel> GetPlayerByEmailAsync(string email)
    {
        // Define a cache key based on the player email
        var cacheKey = $"PlayerModel:{email}";

        // Try to get the player from the cache
        if (!_cache.TryGetValue(cacheKey, out PlayerModel player))
        {
            // If the player is not in the cache, fetch it from the database
            var user = await _userManager.FindByEmailAsync(email);
            if (user != null && user.UserType == "Player")
            {
                player = (PlayerModel)user;

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
    public async Task<IEnumerable<PlayerDTO>> GetAllPlayersAsync()
    {
        // Define a cache key for all players
        var cacheKey = "AllPlayers";

        // Try to get the players from the cache
        if (!_cache.TryGetValue(cacheKey, out IEnumerable<PlayerDTO> playerDTOs))
        {
            // If the players are not in the cache, fetch them from the database
            var players = await _context.Users
                                        .OfType<PlayerModel>()
                                        .Select(p => new PlayerDTO
                                        {
                                            Id = p.Id,
                                            UserName = p.UserName,
                                            Email = p.Email,
                                            FirstName = p.FirstName,
                                            LastName = p.LastName,
                                            AreaOfResidence = p.AreaOfResidence,
                                            DateOfBirth = p.DateOfBirth,
                                            PhoneNumber = p.PhoneNumber,
                                            Status = p.Status,
                                        })
                                        .AsNoTracking()
                                        .ToListAsync();

            // Define cache options
            var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromMinutes(5)) // Cache expiration
                .SetPriority(CacheItemPriority.High);

            // Save the players in the cache
            _cache.Set(cacheKey, players, cacheEntryOptions);
            playerDTOs = players;
        }

        return playerDTOs;
    }
    public async Task<string> CreatePlayerAsync(CreatePlayerDto model, string password)
    {
        // Convert CreatePlayerDto to PlayerModel
        var player = new PlayerModel
        {
            UserName = model.UserName,
            Email = model.Email,
            FirstName = model.FirstName,
            LastName = model.LastName,
            AreaOfResidence = model.AreaOfResidence,
            DateOfBirth = model.DateOfBirth,
            PhoneNumber = model.PhoneNumber,
            Status = model.Status,
            UserType = "Player" // Assuming UserType is a property in your User class
        };

        // Use UserManager to create the user
        var result = await _userManager.CreateAsync(player, password);

        // Check if the user was created successfully
        if (result.Succeeded)
        {
            // Remove the cached list of all players
            _cache.Remove("AllPlayerModels");

            // Add the newly created player to the cache   
            // useful for reducing database calls
            var cacheKey = $"PlayerModel:{player.Id}";
            _cache.Set(cacheKey, player, TimeSpan.FromMinutes(5));

            // Return the user's Id
            return player.Id;
        }
        else
        {
            // Return null or an empty string to indicate failure
            return null;
        }
    }
    public async Task<bool> UpdatePlayerAsync(PlayerModel player)
    {
        // Check if the coach exists and is of UserType "Player"
        var existingPlayer = await _userManager.FindByIdAsync(player.Id);
        if (existingPlayer != null && existingPlayer.UserType == "Player")
        {

            // Update player in the database
            var result = await _userManager.UpdateAsync(player);
            // Clear the cache for the specific player
            _cache.Remove($"CoachModel:{existingPlayer.Id}");
            // Clear the cache for all players
            _cache.Remove("AllPlayers");
            return result.Succeeded;
        }

        return false; // Coach does not exist or is not of UserType "Player"
    }
    public async Task<bool> DeletePlayerAsync(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        // Check if the user exists and is of UserType "Player"
        if (user != null && user.UserType == "Player")
        {
            var result = await _userManager.DeleteAsync(user);
            _cache.Remove($"PlayerModel:{id}"); // Clear the cache for the specific player
            _cache.Remove("AllPlayers"); // Clear the cache for all players
            return result.Succeeded; // Returns true if deletion was successful
        }
        return false; // User does not exist or is not of UserType "Player"
    }
    public async Task<PlayerModel> GetPlayerByUserNameAsync(string userName)
    {
        // Define a cache key based on the player email
        var cacheKey = $"PlayerModel:{userName}";

        // Try to get the player from the cache
        if (!_cache.TryGetValue(cacheKey, out PlayerModel player))
        {
            // If the player is not in the cache, fetch it from the database
            var user = await _userManager.FindByNameAsync(userName);
            if (user != null && user.UserType == "Player")
            {
                player = (PlayerModel)user;

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
    public async Task<PlayerModel> GetPlayerByPhoneNumberAsync(string phoneNumber)
    {
        // Define a cache key based on the organizer phone number
        var cacheKey = $"PlayerModel:{phoneNumber}";

        // Try to get the organizer from the cache
        if (!_cache.TryGetValue(cacheKey, out PlayerModel player))
        {
            // If the organizer is not in the cache, fetch it from the database
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.PhoneNumber == phoneNumber);
            if (user != null && user.UserType == "Player")
            {
                player = (PlayerModel)user;

                // Define cache options
                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromMinutes(5)) // Cache expiration
                    .SetPriority(CacheItemPriority.Normal);

                // Save the organizer in the cache
                _cache.Set(cacheKey, player, cacheEntryOptions);
            }
        }
        return player;
    }
}
