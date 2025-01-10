namespace TransactionAPI.Interfaces;

public interface IPartnerService
{
    bool ValidatePartner(string partnerKey, string password);
    bool ValidateSignature(TransactionRequest request);
}