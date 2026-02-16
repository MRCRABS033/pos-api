using System.ComponentModel.DataAnnotations;

namespace Pos.Application.Dtos.Returns;

public class ReturnItemCreateDto
{
    [Required]
    public Guid SaleItemId { get; set; }

    [Range(typeof(decimal), "0.0001", "999999999")]
    public decimal Quantity { get; set; }
}
