using API.DTO;
using API.Interfaces;

namespace API.Validators
{
    public class RegistrationValidator : IRegistrationValidator
    {
        private readonly IPlayerService _playerService;

        public RegistrationValidator(IPlayerService playerService)
        {
            _playerService = playerService;
        }
        public async Task<ValidationResult> ValidateAsync(CreatePlayerDto model)
        {
            var result = new ValidationResult { IsValid = true };

            if (string.IsNullOrEmpty(model.Email) || string.IsNullOrEmpty(model.UserName) || string.IsNullOrEmpty(model.Password))
            {
                result.IsValid = false;
                result.Errors.Add("Email, UserName, and Password are required.");
            }

            var existingPlayerByUserName = await _playerService.GetPlayerByUserNameAsync(model.UserName);
            if (existingPlayerByUserName != null)
            {
                result.IsValid = false;
                result.Errors.Add("The username is already taken. Please choose a different username.");
            }

            var existingPlayerByEmail = await _playerService.GetPlayerByEmailAsync(model.Email);
            if (existingPlayerByEmail != null)
            {
                result.IsValid = false;
                result.Errors.Add("An account with this email already exists. Please log in.");
            }

            if (!await _playerService.IsPasswordValidAsync(model.Password))
            {
                result.IsValid = false;
                result.Errors.Add("The password does not meet the requirements. It must be at least  8 characters long, contain uppercase and lowercase letters, numbers, and special characters.");
            }

            return result;
        }
    }

}
