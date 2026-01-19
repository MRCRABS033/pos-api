namespace Pos.Application.Dtos.CashFlows;

public class CashFlowCreateDto
{
    public string Motive { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public Guid UserId { get; set; }
}
