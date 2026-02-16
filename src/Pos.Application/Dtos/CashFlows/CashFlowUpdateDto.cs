using System.ComponentModel.DataAnnotations;
using Pos.Domain.Entities;

namespace Pos.Application.Dtos.CashFlows;

public class CashFlowUpdateDto
{
    [Required]
    public Guid Id { get; set; }

    [Required]
    [MinLength(3)]
    [MaxLength(200)]
    public string Motive { get; set; } = string.Empty;

    [Range(typeof(decimal), "0.01", "999999999")]
    public decimal Amount { get; set; }
    public CashFlowType Type { get; set; }

    [Required]
    public Guid CashBoxId { get; set; }
}
