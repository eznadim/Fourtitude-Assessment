using TransactionAPI.Interfaces;
using TransactionAPI.Models;

namespace TransactionAPI.Services;

public class ValidationService : IValidationService
{
    private readonly IPartnerService _partnerService;
    private const int TIMESTAMP_TOLERANCE_MINUTES = 5;

    public ValidationService(IPartnerService partnerService)
    {
        _partnerService = partnerService;
    }

    public async Task<ValidationResult> ValidateRequestAsync(TransactionRequest request)
    {
        // Check for null request
        if (request == null)
        {
            return ValidationResult.Failure("Request body is required.");
        }

        // Validate mandatory fields
        var mandatoryFieldResult = ValidateMandatoryFields(request);
        if (!mandatoryFieldResult.IsValid)
        {
            return mandatoryFieldResult;
        }

        // Validate partner authentication and signature
        if (!_partnerService.ValidatePartner(request.PartnerKey, request.PartnerPassword))
        {
            return ValidationResult.Failure("Access Denied!");
        }

        if (!_partnerService.ValidateSignature(request))
        {
            return ValidationResult.Failure("Access Denied!");
        }

        // Validate timestamp
        var timestampResult = ValidateTimestamp(request.Timestamp);
        if (!timestampResult.IsValid)
        {
            return timestampResult;
        }

        // Validate total amount and items if present
        if (request.Items?.Any() == true)
        {
            var itemsResult = ValidateItems(request);
            if (!itemsResult.IsValid)
            {
                return itemsResult;
            }
        }

        // Validate business rules
        var businessRulesResult = ValidateBusinessRules(request);
        if (!businessRulesResult.IsValid)
        {
            return businessRulesResult;
        }

        return ValidationResult.Success();
    }

    private ValidationResult ValidateMandatoryFields(TransactionRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.PartnerKey))
            return ValidationResult.Failure("partnerkey is Required.");

        if (string.IsNullOrWhiteSpace(request.PartnerRefNo))
            return ValidationResult.Failure("partnerrefno is Required.");

        if (string.IsNullOrWhiteSpace(request.PartnerPassword))
            return ValidationResult.Failure("partnerpassword is Required.");

        if (string.IsNullOrWhiteSpace(request.Timestamp))
            return ValidationResult.Failure("timestamp is Required.");

        if (string.IsNullOrWhiteSpace(request.Sig))
            return ValidationResult.Failure("sig is Required.");

        if (request.TotalAmount <= 0)
            return ValidationResult.Failure("totalamount must be greater than 0.");

        return ValidationResult.Success();
    }

    private ValidationResult ValidateTimestamp(string timestamp)
    {
        if (!DateTime.TryParse(timestamp, out DateTime requestTime))
        {
            return ValidationResult.Failure("Invalid timestamp format.");
        }

        var currentTime = DateTime.UtcNow;
        var timeDifference = (currentTime - requestTime).TotalMinutes;

        if (Math.Abs(timeDifference) > TIMESTAMP_TOLERANCE_MINUTES)
        {
            return ValidationResult.Failure("Expired.");
        }

        return ValidationResult.Success();
    }

    private ValidationResult ValidateItems(TransactionRequest request)
    {
        // Validate individual items
        foreach (var item in request.Items)
        {
            if (string.IsNullOrWhiteSpace(item.PartnerItemRef))
                return ValidationResult.Failure("partneritemref is Required for all items.");

            if (string.IsNullOrWhiteSpace(item.Name))
                return ValidationResult.Failure("item name is Required for all items.");

            if (item.Qty <= 0 || item.Qty > 5)
                return ValidationResult.Failure("item quantity must be between 1 and 5.");

            if (item.UnitPrice <= 0)
                return ValidationResult.Failure("item unitprice must be greater than 0.");
        }

        // Validate total amount matches items
        var calculatedTotal = request.Items.Sum(i => i.Qty * i.UnitPrice);
        if (calculatedTotal != request.TotalAmount)
        {
            return ValidationResult.Failure("Invalid Total Amount.");
        }

        return ValidationResult.Success();
    }

    private ValidationResult ValidateBusinessRules(TransactionRequest request)
    {
        // Additional business validations can be added here
        // For example:
        if (request.TotalAmount > 1000000000) // 10 million in cents
        {
            return ValidationResult.Failure("Transaction amount exceeds maximum limit.");
        }

        // Add more business rules as needed

        return ValidationResult.Success();
    }
}