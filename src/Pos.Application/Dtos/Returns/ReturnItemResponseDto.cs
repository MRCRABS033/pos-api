namespace Pos.Application.Dtos.Returns;

public class ReturnItemResponseDto
{
    public Guid Id { get; set; }
    public Guid ReturnId { get; set; }
    public Guid SaleItemId { get; set; }
    public Guid ProductId { get; set; }
    public string? ProductName { get; set; }
    public decimal Quantity { get; set; }
    public decimal UnitPrice { get; set; }
}
