using API.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.IdentityModel.Tokens;
using ModelsLibrary.DataAccess;
using ModelsLibrary.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace API.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly DataContext _context;
        private readonly IMemoryCache _cache;
        private readonly IConfiguration _configuration;
        public UserRepository(UserManager<User> userManager, SignInManager<User> signInManager, DataContext context, IMemoryCache cache, IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
            _cache = cache;
            _configuration = configuration;
        }
        public async Task SignInAsync(string userId, bool isPersistent)
        {
            var user = await _userManager.FindByIdAsync(userId);
            await _signInManager.SignInAsync(user, isPersistent);
        }
        public async Task<string> GenerateEmailConfirmationTokenAsync(string userId)
        {
            // Find the player by their ID
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                // Handle the case where the player is not found
                // This could involve logging the error or throwing an exception
                throw new ArgumentException("Player not found.", nameof(userId));
            }

            // Generate the email confirmation token for the player
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

            return token;
        }
        public async Task<IdentityResult> ConfirmEmailAsync(string userId, string token)
        {
            // Use the user management system or identity framework to confirm the user's email
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                // User not found
                return IdentityResult.Failed(new IdentityError { Description = "User not found." });
            }

            // Use the user management system or identity framework to confirm the user's email
            var result = await _userManager.ConfirmEmailAsync(user, token);

            return result;
        }
        public async Task<bool> CheckPasswordAsync(string userId, string password)
        {
            var user = await _userManager.FindByIdAsync(userId);
            var result = await _signInManager.CheckPasswordSignInAsync(user, password, false);
            return result.Succeeded;

        }
        public async Task<string> GenerateTwoFactorTokenAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            return await _userManager.GenerateTwoFactorTokenAsync(user, _userManager.Options.Tokens.AuthenticatorTokenProvider);
        }
        public async Task<string> GenerateAuthTokenAsync(string userId)
        {
            // Generated the authentication token using JWT (JSON Web Tokens)
            var user= await _userManager.FindByIdAsync(userId);

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(ClaimTypes.Email,user.Email),
                    new Claim(ClaimTypes.NameIdentifier,user.Id),
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
        public async Task EnableTwoFactorAuthenticationAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            await _userManager.SetTwoFactorEnabledAsync(user, true);
        }

        public async Task DisableTwoFactorAuthenticationAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            await _userManager.SetTwoFactorEnabledAsync(user, false);
        }
        public async Task LogoutAsync()
        {
            await _signInManager.SignOutAsync();
        }
        public async Task<bool> CheckCurrentPasswordAsync(string userId, string currentPassword)
        {
            var user = await _userManager.FindByIdAsync(userId);
            return await _userManager.CheckPasswordAsync(user, currentPassword);
        }
        public async Task<IdentityResult> ChangePasswordAsync(string userId, string currentPassword, string newPassword)
        {
            var user = await _userManager.FindByIdAsync(userId);
            return await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);
        }
    }
}
