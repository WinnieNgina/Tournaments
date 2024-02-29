using API.DTO;
using API.Interfaces;
using API.Services;

namespace API.Validators
{
    public class OrganizerRegistrationValidator : IOrganizerRegistrationValidator
    {
        private readonly IOrganizerService _organizerService;
        public OrganizerRegistrationValidator(IOrganizerService organizerService)
        {

            _organizerService = organizerService;

        }
        public async Task<ValidationResult> ValidateAsync(SignUpOrganizerDTO model)
        {
            var result = new ValidationResult { IsValid = true };

            if (string.IsNullOrEmpty(model.Email) || string.IsNullOrEmpty(model.UserName) || string.IsNullOrEmpty(model.Password) || string.IsNullOrEmpty(model.PhoneNumber))
            {
                result.IsValid = false;
                result.Errors.Add("Email, Phone number, UserName, and Password are required.");
            }

            var existingOrganizerByUserName = await _organizerService.GetOrganizerByNameAsync(model.UserName);
            if (existingOrganizerByUserName != null)
            {
                result.IsValid = false;
                result.Errors.Add("The username is already taken. Please choose a different username.");
            }

            var existingOrganizerByEmail = await _organizerService.GetOrganizerByEmailAsync(model.Email);
            if (existingOrganizerByEmail != null)
            {
                result.IsValid = false;
                result.Errors.Add("An account with this email already exists. Please log in.");
            }
            var existingOrganizerByPhoneNumber = await _organizerService.GetOrganizerByPhoneNumberAsync(model.PhoneNumber);
            if (existingOrganizerByPhoneNumber != null)
            {
                result.IsValid = false;
                result.Errors.Add("An account with this phone number already exists. Please log in.");
            }

            if (!await _organizerService.IsPasswordValidAsync(model.Password))
            {
                result.IsValid = false;
                result.Errors.Add("The password does not meet the requirements. It must be at least  8 characters long, contain uppercase and lowercase letters, numbers, and special characters.");
            }
            return result;
        }
    }
}
