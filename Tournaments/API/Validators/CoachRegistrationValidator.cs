using API.DTO;
using API.Interfaces;
using API.Services;

namespace API.Validators
{
    public class CoachRegistrationValidator : ICoachRegistrationValidator
    {
        private readonly ICoachService _coachService;
        public CoachRegistrationValidator(ICoachService coachService)
        {
            _coachService = coachService;
        }

        public async Task<ValidationResult> ValidateAsync(SignUpCoachDTO model)
        {
            var result = new ValidationResult { IsValid = true };

            if (string.IsNullOrEmpty(model.Email) || string.IsNullOrEmpty(model.UserName) || string.IsNullOrEmpty(model.Password))
            {
                result.IsValid = false;
                result.Errors.Add("Email, UserName, and Password are required.");
            }

            var existingCoachByUserName = await _coachService.GetCoachByNameAsync(model.UserName);
            if (existingCoachByUserName != null)
            {
                result.IsValid = false;
                result.Errors.Add("The username is already taken. Please choose a different username.");
            }

            var existingCoachByEmail = await _coachService.GetCoachByEmailAsync(model.Email);
            if (existingCoachByEmail != null)
            {
                result.IsValid = false;
                result.Errors.Add("An account with this email already exists. Please log in.");
            }

            if (!await _coachService.IsPasswordValidAsync(model.Password))
            {
                result.IsValid = false;
                result.Errors.Add("The password does not meet the requirements. It must be at least  8 characters long, contain uppercase and lowercase letters, numbers, and special characters.");
            }

            return result;
        }
    }
}
