using API.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.IdentityModel.Tokens;
using ModelsLibrary.DataAccess;
using ModelsLibrary.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

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
    public async Task<IEnumerable<PlayerModel>> GetAllPlayersAsync()
    {
        // Define a cache key for all players
        var cacheKey = "AllPlayers";

        // Try to get the players from the cache
        if (!_cache.TryGetValue(cacheKey, out IEnumerable<PlayerModel> playerModels))
        {
            // If the players are not in the cache, fetch them from the database
            var players = await _context.Users
                                        .OfType<PlayerModel>()
                                        .ToListAsync();

            // Cast the results to CoachModel
            playerModels = players.Cast<PlayerModel>();

            // Define cache options
            var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromMinutes(5)) // Cache expiration
                .SetPriority(CacheItemPriority.High);

            // Save the coaches in the cache
            _cache.Set(cacheKey, playerModels, cacheEntryOptions);
        }

        return playerModels;
    }
    public async Task<string> CreatePlayerAsync(PlayerModel player, string password)
    {
        // Ensure the player object is correctly set up before creating the user
        var user = new PlayerModel
        {
            UserName = player.UserName,
            Email = player.Email,
            FirstName = player.FirstName,
            LastName = player.LastName,
            AreaOfResidence = player.AreaOfResidence,
            DateOfBirth = player.DateOfBirth,
            Status = player.Status,
            UserType = "Player" // Assuming UserType is a property in your User class
        };

        // Use UserManager to create the user
        var result = await _userManager.CreateAsync(user, password);

        // Check if the user was created successfully
        if (result.Succeeded)
        {

            // Remove the cached list of all players
            _cache.Remove("AllPlayerModels");

            // Add the newly created player to the cache   
            // useful for reducing database calls
            var cacheKey = $"PlayerModel:{user.Id}";
            _cache.Set(cacheKey, user, TimeSpan.FromMinutes(5));

            // Return the user's Id
            return user.Id;
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
    public async Task<string> GenerateEmailConfirmationTokenAsync(string playerId)
    {
        // Find the player by their ID
        var player = await _userManager.FindByIdAsync(playerId);

        if (player == null)
        {
            // Handle the case where the player is not found
            // This could involve logging the error or throwing an exception
            throw new ArgumentException("Player not found.", nameof(playerId));
        }

        // Generate the email confirmation token for the player
        var token = await _userManager.GenerateEmailConfirmationTokenAsync(player);

        return token;
    }
    public async Task<IdentityResult> ConfirmEmailAsync(string playerId, string token)
    {
        // Use the user management system or identity framework to confirm the user's email
        var player = await _userManager.FindByIdAsync(playerId);
        if (player == null)
        {
            // User not found
            return IdentityResult.Failed(new IdentityError { Description = "Player not found." });
        }

        // Use the user management system or identity framework to confirm the user's email
        var result = await _userManager.ConfirmEmailAsync(player, token);

        return result;
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
    public async Task<bool> CheckPasswordAsync(string playerId, string password)
    {
        var player = await _userManager.FindByIdAsync(playerId);
        var result = await _signInManager.CheckPasswordSignInAsync(player, password, false);
        return result.Succeeded;

    }
    public async Task<string> GenerateTwoFactorTokenAsync(string playerId)
    {
        var player = await _userManager.FindByIdAsync(playerId);
        return await _userManager.GenerateTwoFactorTokenAsync(player, _userManager.Options.Tokens.AuthenticatorTokenProvider);
    }
    public async Task<string> GenerateAuthTokenAsync(string playerId)
    {
        // Generated the authentication token using JWT (JSON Web Tokens)
        var player = await _userManager.FindByIdAsync(playerId);

        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
                    new Claim(ClaimTypes.Name, player.UserName),
                    new Claim(ClaimTypes.Email, player.Email),
                    new Claim(ClaimTypes.NameIdentifier,player.Id),
                    new Claim(JwtRegisteredClaimNames.Iss, _configuration["Jwt:Issuer"]),
                    new Claim(JwtRegisteredClaimNames.Aud, _configuration["Jwt:Audience"])
            }),
            Expires = DateTime.UtcNow.AddHours(1),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        var tokenString = tokenHandler.WriteToken(token);

        return tokenString;
    }
    public async Task EnableTwoFactorAuthenticationAsync(string playerId)
    {
        var player = await _userManager.FindByIdAsync(playerId);
        await _userManager.SetTwoFactorEnabledAsync(player, true);
    }

    public async Task DisableTwoFactorAuthenticationAsync(string playerId)
    {
        var player = await _userManager.FindByIdAsync(playerId);
        await _userManager.SetTwoFactorEnabledAsync(player, false);
    }
    public async Task LogoutAsync()
    {
        await _signInManager.SignOutAsync();
    }
    public async Task<bool> CheckCurrentPasswordAsync(string playerId, string currentPassword)
    {
        var player = await _userManager.FindByIdAsync(playerId);
        return await _userManager.CheckPasswordAsync(player, currentPassword);
    }
    public async Task<IdentityResult> ChangePasswordAsync(string playerId, string currentPassword, string newPassword)
    {
        var player = await _userManager.FindByIdAsync(playerId);
        return await _userManager.ChangePasswordAsync(player, currentPassword, newPassword);
    }
    public async Task SignInAsync(string playerId, bool isPersistent)
    {
        var player = await _userManager.FindByIdAsync(playerId);
        await _signInManager.SignInAsync(player, isPersistent);
    }
}
