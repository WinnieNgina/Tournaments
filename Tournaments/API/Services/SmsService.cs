using API.Interfaces;
using API.Options;
using Microsoft.Extensions.Options;
using AfricasTalkingCS;

namespace API.Services;

public class SmsService : ISmsService
{
    private readonly string _username;
    private readonly string _apiKey;
    public SmsService(IOptions<SmsOptions> smsOptions, string apiKey)
    {
        var options = smsOptions.Value;
        _username = options.Username;
        _apiKey = apiKey;
    }
    public void SendSms(string phoneNumber, string message)
    {

        var gateway = new AfricasTalkingGateway(_username, _apiKey);

        var sms = gateway.SendMessage(phoneNumber, message);
    }
}