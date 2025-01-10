using TransactionAPI.Interfaces;

namespace TransactionAPI.Services;

public class DiscountService : IDiscountService
{
    private const decimal MAX_DISCOUNT_PERCENTAGE = 0.20m; // 20%

    public (long totalDiscount, long finalAmount) CalculateDiscount(long totalAmount)
    {
        decimal amountInRM = totalAmount / 100m; // Convert cents to RM
        decimal discountPercentage = CalculateBaseDiscount(amountInRM);
        
        // Add conditional discounts
        discountPercentage += CalculateConditionalDiscounts(amountInRM);
        
        // Cap the discount at maximum allowed percentage
        discountPercentage = Math.Min(discountPercentage, MAX_DISCOUNT_PERCENTAGE);
        
        // Calculate final amounts in cents
        long totalDiscountCents = (long)(totalAmount * discountPercentage);
        long finalAmountCents = totalAmount - totalDiscountCents;
        
        return (totalDiscountCents, finalAmountCents);
    }

    private decimal CalculateBaseDiscount(decimal amountInRM)
    {
        if (amountInRM <= 200) return 0;
        if (amountInRM <= 500) return 0.05m;
        if (amountInRM <= 800) return 0.07m;
        if (amountInRM <= 1200) return 0.10m;
        return 0.15m;
    }

    private decimal CalculateConditionalDiscounts(decimal amountInRM)
    {
        decimal additionalDiscount = 0;

        // Check for prime number condition
        if (amountInRM > 500 && IsPrimeNumber((long)(amountInRM * 100)))
        {
            additionalDiscount += 0.08m;
        }

        // Check for ending in 5 condition
        if (amountInRM > 900 && ((long)(amountInRM * 100) % 1000) % 10 == 5)
        {
            additionalDiscount += 0.10m;
        }

        return additionalDiscount;
    }

    private bool IsPrimeNumber(long number)
    {
        if (number <= 1) return false;
        if (number == 2) return true;
        if (number % 2 == 0) return false;

        var boundary = (long)Math.Floor(Math.Sqrt(number));
        
        for (long i = 3; i <= boundary; i += 2)
        {
            if (number % i == 0) return false;
        }

        return true;
    }
}
