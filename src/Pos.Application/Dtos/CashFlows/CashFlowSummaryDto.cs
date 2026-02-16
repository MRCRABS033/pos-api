namespace Pos.Application.Dtos.CashFlows;

public class CashFlowSummaryDto
{
    public Guid CashBoxId { get; set; }
    public int EntriesCount { get; set; }
    public int ExitsCount { get; set; }
    public decimal EntriesTotal { get; set; }
    public decimal ExitsTotal { get; set; }
    public decimal Total { get; set; }
}
