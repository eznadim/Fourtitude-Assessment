using System.ComponentModel.DataAnnotations;

public class ItemDetail
{
    [Required(ErrorMessage = "Partner item reference is required")]
    [StringLength(50)]
    public string PartnerItemRef { get; set; }

    [Required(ErrorMessage = "Item name is required")]
    [StringLength(100)]
    public string Name { get; set; }

    [Range(1, 5, ErrorMessage = "Quantity must be between 1 and 5")]
    public int Qty { get; set; }

    [Range(1, long.MaxValue, ErrorMessage = "Unit price must be positive")]
    public long UnitPrice { get; set; }
}

public class TransactionRequest
{
    [Required(ErrorMessage = "Partner key is required")]
    [StringLength(50)]
    public string PartnerKey { get; set; }

    [Required(ErrorMessage = "Partner reference number is required")]
    [StringLength(50)]
    public string PartnerRefNo { get; set; }

    [Required(ErrorMessage = "Partner password is required")]
    [StringLength(50)]
    public string PartnerPassword { get; set; }

    [Range(1, long.MaxValue, ErrorMessage = "Total amount must be positive")]
    public long TotalAmount { get; set; }

    public List<ItemDetail> Items { get; set; }

    [Required(ErrorMessage = "Timestamp is required")]
    public string Timestamp { get; set; }

    [Required(ErrorMessage = "Signature is required")]
    public string Sig { get; set; }
}

public class TransactionResponse
{
    public int Result { get; set; }
    public long? TotalAmount { get; set; }
    public long? TotalDiscount { get; set; }
    public long? FinalAmount { get; set; }
    public string ResultMessage { get; set; }

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