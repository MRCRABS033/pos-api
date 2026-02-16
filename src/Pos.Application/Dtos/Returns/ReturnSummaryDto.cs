namespace Pos.Application.Dtos.Returns;

public class ReturnSummaryDto
{
    public Guid CashBoxId { get; set; }
    public int ReturnsCount { get; set; }
    public decimal CashTotal { get; set; }
    public decimal CardTotal { get; set; }
    public decimal Total { get; set; }
}
