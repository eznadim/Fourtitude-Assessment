using TransactionAPI.Models;

namespace TransactionAPI.Interfaces;

public interface IValidationService
{
    Task<ValidationResult> ValidateRequestAsync(TransactionRequest request);
}