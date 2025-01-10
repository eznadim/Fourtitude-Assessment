namespace TransactionAPI.Models;

public class TransactionResponse
{
    public int Result { get; set; }
    public long? TotalAmount { get; set; }
    public long? TotalDiscount { get; set; }
    public long? FinalAmount { get; set; }
    public string? ResultMessage { get; set; }

    public static TransactionResponse Success(long totalAmount, long totalDiscount, long finalAmount)
    {
        return new TransactionResponse
        {
            Result = 1,
            TotalAmount = totalAmount,
            TotalDiscount = totalDiscount,
            FinalAmount = finalAmount
        };
    }

    public static TransactionResponse Failure(string message)
    {
        return new TransactionResponse
        {
            Result = 0,
            ResultMessage = message
        };
    }
}