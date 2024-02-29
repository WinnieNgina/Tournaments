using API.DTO;
using API.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using ModelsLibrary.DataAccess;
using ModelsLibrary.Models;

namespace API.Repository
{
    public class OrganizerRepository : IOrganizerRepository
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly DataContext _context;
        private readonly IMemoryCache _cache;
        public OrganizerRepository(UserManager<User> userManager, SignInManager<User> signInManager, DataContext context, IMemoryCache cache)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
            _cache = cache;
        }
        public async Task<string> CreateOrganizerAsync(TournamentOrganizerModel organizer, string password)
        {
            // Convert CoachModel to User or ApplicationIdentityUser based on your setup
            var user = new TournamentOrganizerModel
            {
                PhoneNumber = organizer.PhoneNumber,
                FirstName = organizer.FirstName,
                LastName = organizer.LastName,
                AreaOfResidence = organizer.AreaOfResidence,
                UserName = organizer.UserName,
                Email = organizer.Email,
                OrganizationName = organizer.OrganizationName,
                UserType = "TournamentOrganizer" // Assuming UserType is a property in your User class
            };

            // Use UserManager to create the user
            var result = await _userManager.CreateAsync(user, password);

            // Check if the user was created successfully
            if (result.Succeeded)
            {
                // Remove the cached list of all players
                _cache.Remove("AllOrganizers");

                // Optionally, you can also add the newly created coach to the cache if needed
                // This is not necessary for invalidating the cache but can be useful for reducing database calls
                var cacheKey = $"Organizer:{user.Id}";
                _cache.Set(cacheKey, organizer, TimeSpan.FromMinutes(5));
                return user.Id;
            }
            else
            {
                // Return null or an empty string to indicate failure
                return null;
            }
        }

        public async Task<bool> DeleteOrganizerAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            // Check if the user exists and is of UserType "Orgnaizer"
            if (user != null && user.UserType == "TournamentOrganizer")
            {
                var result = await _userManager.DeleteAsync(user);
                _cache.Remove($"Organizer:{id}"); // Clear the cache for the specific organizer
                _cache.Remove("AllOrganizers"); // Clear the cache for all organizers
                return result.Succeeded; // Returns true if deletion was successful
            }
            return false; // User does not exist or is not of UserType "Organizer"
        }

        public async Task<IEnumerable<OrganizerDTO>> GetAllOrganizersAsync()
        {
            // Define a cache key for all organizers
            var cacheKey = "AllOrganizers";

            // Try to get the organizers from the cache
            if (!_cache.TryGetValue(cacheKey, out IEnumerable<OrganizerDTO> organizerDTOs))
            {
                // If the organizers are not in the cache, fetch them from the database
                // Select only necessary fields and use AsNoTracking for performance
                var organizers = await _context.Users
                                .OfType<TournamentOrganizerModel>()
                                .Select(o => new OrganizerDTO
                                {
                                    Id = o.Id,
                                    UserName = o.UserName,
                                    Email = o.Email,
                                    FirstName = o.FirstName,
                                    LastName = o.LastName,
                                    AreaOfResidence = o.AreaOfResidence,
                                    OrganizationName = o.OrganizationName,
                                })
                                .AsNoTracking()
                                .ToListAsync();

                // Save the coaches in the cache
                _cache.Set(cacheKey, organizers, new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromMinutes(5)) // Cache expiration
                    .SetPriority(CacheItemPriority.High));

                organizerDTOs = organizers;
            }

            return organizerDTOs;
        }
        public async Task<TournamentOrganizerModel> GetOrganizerByEmailAsync(string email)
        {
            // Define a cache key based on the organizer email
            var cacheKey = $"Organizer:{email}";

            // Try to get the organizer from the cache
            if (!_cache.TryGetValue(cacheKey, out TournamentOrganizerModel organizer))
            {
                // If the organizer is not in the cache, fetch it from the database
                var user = await _userManager.FindByEmailAsync(email);
                if (user != null && user.UserType == "TournamentOrganizer")
                {
                    organizer = (TournamentOrganizerModel)user;

                    // Define cache options
                    var cacheEntryOptions = new MemoryCacheEntryOptions()
                        .SetSlidingExpiration(TimeSpan.FromMinutes(5)) // Cache expiration
                        .SetPriority(CacheItemPriority.Normal);

                    // Save the organizer in the cache
                    _cache.Set(cacheKey, organizer, cacheEntryOptions);
                }
            }
            return organizer;
        }
        public async Task<TournamentOrganizerModel> GetOrganizerByIdAsync(string id)
        {
            // Define a cache key based on the organizer ID
            var cacheKey = $"Organizer:{id}";

            // Try to get the organizer from the cache
            if (!_cache.TryGetValue(cacheKey, out TournamentOrganizerModel organizer))
            {
                // If the organizer is not in the cache, fetch it from the database
                var user = await _userManager.FindByIdAsync(id);
                if (user != null && user.UserType == "TournamentOrganizer")
                {
                    organizer = (TournamentOrganizerModel)user;

                    // Define cache options
                    var cacheEntryOptions = new MemoryCacheEntryOptions()
                        .SetSlidingExpiration(TimeSpan.FromMinutes(5)) // Cache expiration
                        .SetPriority(CacheItemPriority.Normal);

                    // Save the organizer in the cache
                    _cache.Set(cacheKey, organizer, cacheEntryOptions);
                }
            }
            return organizer;
        }
        public async Task<TournamentOrganizerModel> GetOrganizerByNameAsync(string userName)
        {
            // Define a cache key based on the organizer user name
            var cacheKey = $"Organizer:{userName}";

            // Try to get the organizer from the cache
            if (!_cache.TryGetValue(cacheKey, out TournamentOrganizerModel organizer))
            {
                // If the organizer is not in the cache, fetch it from the database
                var user = await _userManager.FindByNameAsync(userName);
                if (user != null && user.UserType == "TournamentOrganizer")
                {
                    organizer = (TournamentOrganizerModel)user;

                    // Define cache options
                    var cacheEntryOptions = new MemoryCacheEntryOptions()
                        .SetSlidingExpiration(TimeSpan.FromMinutes(5)) // Cache expiration
                        .SetPriority(CacheItemPriority.Normal);

                    // Save the organizer in the cache
                    _cache.Set(cacheKey, organizer, cacheEntryOptions);
                }
            }
            return organizer;
        }

        public async Task<TournamentOrganizerModel> GetOrganizerByPhoneNumberAsync(string phoneNumber)
        {
            // Define a cache key based on the organizer phone number
            var cacheKey = $"Organizer:{phoneNumber}";

            // Try to get the organizer from the cache
            if (!_cache.TryGetValue(cacheKey, out TournamentOrganizerModel organizer))
            {
                // If the organizer is not in the cache, fetch it from the database
                var user = await _userManager.Users.FirstOrDefaultAsync(u => u.PhoneNumber == phoneNumber);
                if (user != null && user.UserType == "TournamentOrganizer")
                {
                    organizer = (TournamentOrganizerModel)user;

                    // Define cache options
                    var cacheEntryOptions = new MemoryCacheEntryOptions()
                        .SetSlidingExpiration(TimeSpan.FromMinutes(5)) // Cache expiration
                        .SetPriority(CacheItemPriority.Normal);

                    // Save the organizer in the cache
                    _cache.Set(cacheKey, organizer, cacheEntryOptions);
                }
            }
            return organizer;
        }

        public async Task<bool> UpdateOrganizerAsync(TournamentOrganizerModel organizer)
        {
            // Check if the coach exists and is of UserType "Organizer"
            var existingOrganizer = await _userManager.FindByIdAsync(organizer.Id);
            if (existingOrganizer != null && existingOrganizer.UserType == "TournamentOrganizer")
            {

                // Update organizer in the database
                var result = await _userManager.UpdateAsync(organizer);
                // Clear the cache for the specific organizer
                _cache.Remove($"Organizer:{existingOrganizer.Id}");
                // Clear the cache for all organizers
                _cache.Remove("AllOrganizers");
                return result.Succeeded;
            }
            return false; // Tournament Organizer does not exist or is not of UserType "Organizer"
        }
    }
}
