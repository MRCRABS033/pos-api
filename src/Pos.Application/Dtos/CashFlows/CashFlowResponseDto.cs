using Pos.Domain.Entities;

namespace Pos.Application.Dtos.CashFlows;

public class CashFlowResponseDto
{
    public Guid Id { get; set; }
    public string Motive { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public CashFlowType Type { get; set; }
    public Guid UserId { get; set; }
    public Guid? CashBoxId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
