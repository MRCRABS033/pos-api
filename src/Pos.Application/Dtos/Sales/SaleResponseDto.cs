using Pos.Domain.Entities;
using Pos.Application.Dtos.SaleItems;

namespace Pos.Application.Dtos.Sales;

public class SaleResponseDto
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid? CashBoxId { get; set; }
    public PaymentType PaymentType { get; set; }
    public decimal Total { get; set; }
    public decimal Discount { get; set; }
    public int ItemsCount { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public List<SaleItemResponseDto> Items { get; set; } = new();
}
