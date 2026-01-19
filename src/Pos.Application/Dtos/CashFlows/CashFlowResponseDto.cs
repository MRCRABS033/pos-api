namespace Pos.Application.Dtos.CashFlows;

public class CashFlowResponseDto
{
    public Guid Id { get; set; }
    public string Motive { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public Guid UserId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
