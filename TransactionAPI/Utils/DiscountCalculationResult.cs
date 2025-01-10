namespace TransactionAPI.Utils;

public class DiscountCalculationResult
{
    public long TotalAmount { get; set; }
    public long TotalDiscount { get; set; }
    public long FinalAmount { get; set; }
    public List<string> AppliedDiscounts { get; set; } = new();
}