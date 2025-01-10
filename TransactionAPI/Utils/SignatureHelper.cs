using System.Security.Cryptography;
using System.Text;

public static class SignatureHelper
{
    public static string ComputeSignature(string timestamp, string partnerKey, 
        string partnerRefNo, long totalAmount, string encodedPassword)
    {
        var valueToHash = $"{timestamp}{partnerKey}{partnerRefNo}{totalAmount}{encodedPassword}";
        using var sha256 = SHA256.Create();
        var hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(valueToHash));
        return Convert.ToBase64String(hashBytes);
    }
}