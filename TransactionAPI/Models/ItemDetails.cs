using System.ComponentModel.DataAnnotations;

namespace TransactionAPI.Models;

public class ItemDetail
{
    [Required(ErrorMessage = "Partner item reference is required")]
    [StringLength(50)]
    public string PartnerItemRef { get; set; } = string.Empty;

    [Required(ErrorMessage = "Item name is required")]
    [StringLength(100)]
    public string Name { get; set; } = string.Empty;

    [Range(1, 5, ErrorMessage = "Quantity must be between 1 and 5")]
    public int Qty { get; set; }

    [Range(1, long.MaxValue, ErrorMessage = "Unit price must be positive")]
    public long UnitPrice { get; set; }
}