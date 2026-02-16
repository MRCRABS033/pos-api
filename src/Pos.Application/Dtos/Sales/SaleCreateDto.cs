using System.ComponentModel.DataAnnotations;
using Pos.Application.Dtos.SaleItems;
using Pos.Domain.Entities;

namespace Pos.Application.Dtos.Sales;

public class SaleCreateDto
{
    [Required]
    public Guid CashBoxId { get; set; }
    public PaymentType PaymentType { get; set; }

    [Range(typeof(decimal), "0", "999999999")]
    public decimal Discount { get; set; }

    [MinLength(1)]
    public List<SaleItemCreateDto> Items { get; set; } = new();
}
