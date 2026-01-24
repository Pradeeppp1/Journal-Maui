using Microsoft.Maui.Storage;

namespace Journal.Services;

public class SecurityService : ISecurityService
{
    private const string PinKey = "user_pin_code";
    
    public bool IsAuthenticated { get; set; } = false;

    public bool IsPinSet()
    {
        return Preferences.Default.ContainsKey(PinKey);
    }

    public bool VerifyPin(string pin)
    {
        var savedPin = Preferences.Default.Get(PinKey, string.Empty);
        return savedPin == pin;
    }

    public void SetPin(string pin)
    {
        Preferences.Default.Set(PinKey, pin);
    }

    public void ClearPin()
    {
        Preferences.Default.Remove(PinKey);
        IsAuthenticated = false;
    }
}
