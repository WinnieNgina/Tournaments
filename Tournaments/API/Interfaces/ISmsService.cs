namespace API.Interfaces;

public interface ISmsService
{
    void SendSms(string phoneNumber, string message);
}
