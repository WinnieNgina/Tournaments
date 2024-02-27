using API.DTO;

namespace API.Interfaces;

public interface ILoginValidatorFactory
{
    ILoginValidator CreateValidator(string role);
}
