using Microsoft.AspNetCore.Mvc;
using TransactionAPI.Interfaces;

[ApiController]
[Route("api")]
public class TransactionController : ControllerBase
{
    private readonly IValidationService _validationService;
    private readonly IDiscountService _discountService;
    private readonly ILoggingService _loggingService;

    public TransactionController(
        IValidationService validationService,
        IDiscountService discountService,
        ILoggingService loggingService)
    {
        _validationService = validationService;
        _discountService = discountService;
        _loggingService = loggingService;
    }

    [HttpPost("submittrxmessage")]
    public async Task<ActionResult<TransactionResponse>> SubmitTransaction([FromBody] TransactionRequest request)
    {
        try
        {
            _loggingService.LogRequest(request);

            var validationResult = await _validationService.ValidateRequestAsync(request);
            if (!validationResult.IsValid)
            {
                var errorResponse = TransactionResponse.Failure(validationResult.ErrorMessage);
                _loggingService.LogResponse(errorResponse);
                return Ok(errorResponse);
            }

            var (totalDiscount, finalAmount) = _discountService.CalculateDiscount(request.TotalAmount);
            var response = new TransactionResponse
            {
                Result = 1,
                TotalAmount = request.TotalAmount,
                TotalDiscount = totalDiscount,
                FinalAmount = finalAmount
            };

            _loggingService.LogResponse(response);
            return Ok(response);
        }
        catch (Exception ex)
        {
            _loggingService.LogError("Error processing transaction", ex);
            var errorResponse = TransactionResponse.Failure("Internal server error");
            _loggingService.LogResponse(errorResponse);
            return Ok(errorResponse);
        }
    }
}