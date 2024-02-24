using API.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using ModelsLibrary.DataAccess;
using ModelsLibrary.Models;

public class CoachRepository : ICoachRepository
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly DataContext _context;
    private readonly IMemoryCache _cache;

    public CoachRepository(UserManager<User> userManager, SignInManager<User> signInManager, DataContext context, IMemoryCache cache)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _context = context;
        _cache = cache;
    }

    public async Task<CoachModel> GetCoachByIdAsync(string id)
    {
        // Define a cache key based on the coach ID
        var cacheKey = $"CoachModel:{id}";

        // Try to get the coach from the cache
        if (!_cache.TryGetValue(cacheKey, out CoachModel coach))
        {
            // If the coach is not in the cache, fetch it from the database
            var user = await _userManager.FindByIdAsync(id);
            if (user != null && user.UserType == "Coach")
            {
                coach = (CoachModel)user;

                // Define cache options
                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromMinutes(5)) // Cache expiration
                    .SetPriority(CacheItemPriority.Normal);

                // Save the coach in the cache
                _cache.Set(cacheKey, coach, cacheEntryOptions);
            }
        }

        return coach;
    }
    public async Task<CoachModel> GetCoachByEmailAsync(string email)
    {
        // Define a cache key based on the coach email
        var cacheKey = $"CoachModel:{email}";

        // Try to get the coach from the cache
        if (!_cache.TryGetValue(cacheKey, out CoachModel coach))
        {
            // If the coach is not in the cache, fetch it from the database
            var user = await _userManager.FindByEmailAsync(email);
            if (user != null && user.UserType == "Coach")
            {
                // Cast the user directly to CoachModel since we know UserType is "Coach"
                coach = (CoachModel)user;

                // Define cache options
                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromMinutes(5)) // Cache expiration
                    .SetPriority(CacheItemPriority.Normal);

                // Save the coach in the cache
                _cache.Set(cacheKey, coach, cacheEntryOptions);
            }
        }

        return coach;
    }
    public async Task<IEnumerable<CoachModel>> GetAllCoachesAsync()
    {
        // Define a cache key for all coaches
        var cacheKey = "AllCoachModels";

        // Try to get the coaches from the cache
        if (!_cache.TryGetValue(cacheKey, out IEnumerable<CoachModel> coachModels))
        {
            // If the coaches are not in the cache, fetch them from the database
            var coaches = await _context.Users
                                        .OfType<CoachModel>()
                                        .ToListAsync();

            // Cast the results to CoachModel
            coachModels = coaches.Cast<CoachModel>();

            // Define cache options
            var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromMinutes(5)) // Cache expiration
                .SetPriority(CacheItemPriority.High);

            // Save the coaches in the cache
            _cache.Set(cacheKey, coachModels, cacheEntryOptions);
        }

        return coachModels;
    }


    public async Task CreateCoachAsync(CoachModel coach, string password)
    {
        // Convert CoachModel to User or ApplicationIdentityUser based on your setup
        var user = new CoachModel
        {
            FirstName = coach.FirstName,
            LastName = coach.LastName,
            AreaOfResidence = coach.AreaOfResidence,
            UserName = coach.UserName,
            Email = coach.Email,
            SocialMediaUrl = coach.SocialMediaUrl,
            CoachingSpecialization = coach.CoachingSpecialization,
            UserType = "Coach" // Assuming UserType is a property in your User class
        };

        // Use UserManager to create the user
        var result = await _userManager.CreateAsync(user, password);

        // Check if the user was created successfully
        if (result.Succeeded)
        {
            // Remove the cached list of all players
            _cache.Remove("AllCoachModels");

            // Optionally, you can also add the newly created coach to the cache if needed
            // This is not necessary for invalidating the cache but can be useful for reducing database calls
            var cacheKey = $"CoachModel:{user.Id}";
            _cache.Set(cacheKey, coach, TimeSpan.FromMinutes(5));
        }
        else
        {
            // Handle the failure (e.g., throw an exception or return an error)
            throw new Exception("Failed to add player");
        }
    }

    public async Task<bool> UpdateCoachAsync(CoachModel coach)
    {
        // Check if the coach exists and is of UserType "Coach"
        var existingCoach = await _userManager.FindByIdAsync(coach.Id);
        if (existingCoach != null && existingCoach.UserType == "Coach")
        {

            // Update coach in the database
            var result = await _userManager.UpdateAsync(coach);
            // Clear the cache for the specific coach
            _cache.Remove($"CoachModel:{existingCoach.Id}");
            // Clear the cache for all coaches
            _cache.Remove("AllCoaches");
            return result.Succeeded;
        }

        return false; // Coach does not exist or is not of UserType "Coach"
    }


    public async Task<bool> DeleteCoachAsync(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        // Check if the user exists and is of UserType "Coach"
        if (user != null && user.UserType == "Coach")
        {
            var result = await _userManager.DeleteAsync(user);
            _cache.Remove($"CoachModel:{id}"); // Clear the cache for the specific coach
            _cache.Remove("AllCoaches"); // Clear the cache for all coaches
            return result.Succeeded; // Returns true if deletion was successful
        }
        return false; // User does not exist or is not of UserType "Coach"
    }

}
