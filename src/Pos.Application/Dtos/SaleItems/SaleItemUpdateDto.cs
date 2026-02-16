using System.ComponentModel.DataAnnotations;

namespace Pos.Application.Dtos.SaleItems;

public class SaleItemUpdateDto
{
    [Required]
    public Guid Id { get; set; }

    [Required]
    public Guid SaleId { get; set; }

    [Required]
    public Guid ProductId { get; set; }

    [Range(typeof(decimal), "0.0001", "999999999")]
    public decimal Quantity { get; set; }

    [Range(typeof(decimal), "0", "999999999")]
    public decimal UnitPrice { get; set; }
}
