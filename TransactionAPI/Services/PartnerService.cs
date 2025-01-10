using System.Security.Cryptography;
using System.Text;
using TransactionAPI.Interfaces;

public class PartnerService : IPartnerService
{
    private readonly Dictionary<string, (string Key, string Password)> _partners = new()
    {
        { "FG-00001", ("FAKEGOOGLE", "FAKEPASSWORD1234") },
        { "FG-00002", ("FAKEPEOPLE", "FAKEPASSWORD4578") }
    };

    public bool ValidatePartner(string partnerKey, string password)
    {
        if (string.IsNullOrEmpty(partnerKey) || string.IsNullOrEmpty(password))
            return false;

        var decodedPassword = Encoding.UTF8.GetString(Convert.FromBase64String(password));
        return _partners.Any(p => p.Value.Key == partnerKey && p.Value.Password == decodedPassword);
    }

    public bool ValidateSignature(TransactionRequest request)
    {
        try
        {
            var timestampUtc = DateTime.Parse(request.Timestamp).ToUniversalTime();
            var sigTimestamp = timestampUtc.ToString("yyyyMMddHHmmss");

            var valueToHash = $"{sigTimestamp}{request.PartnerKey}{request.PartnerRefNo}" +
                            $"{request.TotalAmount}{request.PartnerPassword}";

            using var sha256 = SHA256.Create();
            var hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(valueToHash));
            var computedSignature = Convert.ToBase64String(hashBytes);

            return computedSignature.Trim() == request.Sig.Trim();
        }
        catch
        {
            return false;
        }
    }
}