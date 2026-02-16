using Pos.Domain.Entities;

namespace Pos.Application.Dtos.Returns;

public class ReturnResponseDto
{
    public Guid Id { get; set; }
    public Guid SaleId { get; set; }
    public Guid UserId { get; set; }
    public DateTime CreatedAt { get; set; }
    public string Reason { get; set; } = string.Empty;
    public decimal Total { get; set; }
    public PaymentType PaymentType { get; set; }
    public List<ReturnItemResponseDto> Items { get; set; } = new();
}
