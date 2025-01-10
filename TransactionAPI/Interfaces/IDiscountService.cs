namespace TransactionAPI.Interfaces;

public interface IDiscountService
{
    (long totalDiscount, long finalAmount) CalculateDiscount(long totalAmount);
}