using System.ComponentModel.DataAnnotations;

namespace TransactionAPI.Models;

public class TransactionRequest
{
    [Required(ErrorMessage = "Partner key is required")]
    [StringLength(50)]
    public string PartnerKey { get; set; } = string.Empty;

    [Required(ErrorMessage = "Partner reference number is required")]
    [StringLength(50)]
    public string PartnerRefNo { get; set; } = string.Empty;

    [Required(ErrorMessage = "Partner password is required")]
    [StringLength(50)]
    public string PartnerPassword { get; set; } = string.Empty;

    [Range(1, long.MaxValue, ErrorMessage = "Total amount must be positive")]
    public long TotalAmount { get; set; }

    public List<ItemDetail> Items { get; set; } = new();

    [Required(ErrorMessage = "Timestamp is required")]
    public string Timestamp { get; set; } = string.Empty;

    [Required(ErrorMessage = "Signature is required")]
    public string Sig { get; set; } = string.Empty;
}
