namespace Journal.Services;

public interface ISecurityService
{
    bool IsPinSet();
    bool VerifyPin(string pin);
    void SetPin(string pin);
    void ClearPin();
    bool IsAuthenticated { get; set; }
}
