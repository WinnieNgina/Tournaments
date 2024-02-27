using API.DTO;
using API.Interfaces;

namespace API.Validators;

public class LoginValidatorFactory : ILoginValidatorFactory
{
    private readonly IServiceProvider _serviceProvider;

    public LoginValidatorFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }
    public ILoginValidator CreateValidator(string role)
    {
        switch (role)
        {
            case "Coach":
                return _serviceProvider.GetRequiredService<CoachLoginValidator>();
            case "Player":
                return _serviceProvider.GetRequiredService<PlayerLoginValidator>();
            default:
                throw new ArgumentException("Invalid role", nameof(role));
        }
    }
}
