using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pos.Application.Dtos.CashFlows;
using Pos.Application.Interfaces.Services;
using Pos.Domain.Security;

namespace Pos.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CashFlowsController : ControllerBase
{
    private readonly ICashFlowService _cashFlowService;

    public CashFlowsController(ICashFlowService cashFlowService)
    {
        _cashFlowService = cashFlowService;
    }

    [HttpGet("{id:guid}")]
    [Authorize(Policy = PermissionCodes.CashFlowsRead)]
    public async Task<ActionResult<CashFlowResponseDto>> GetById(Guid id)
    {
        var cashFlow = await _cashFlowService.GetByIdAsync(id);
        return Ok(cashFlow);
    }

    [HttpGet]
    [Authorize(Policy = PermissionCodes.CashFlowsRead)]
    public async Task<ActionResult<IReadOnlyList<CashFlowResponseDto>>> GetAll(
        [FromQuery] Guid? userId,
        [FromQuery] Guid? cashBoxId,
        [FromQuery] DateTime? startDate,
        [FromQuery] DateTime? endDate,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 50)
    {
        if (userId.HasValue)
        {
            var byUser = await _cashFlowService.GetByUserIdAsync(userId.Value);
            return Ok(byUser);
        }

        if (cashBoxId.HasValue)
        {
            var byCashBox = await _cashFlowService.GetByCashBoxIdAsync(cashBoxId.Value);
            return Ok(byCashBox);
        }

        if (startDate.HasValue && endDate.HasValue)
        {
            var byDate = await _cashFlowService.GetByDateRangeAsync(startDate.Value, endDate.Value);
            return Ok(byDate);
        }

        var cashFlows = await _cashFlowService.GetAllAsync(page, pageSize);
        return Ok(cashFlows);
    }

    [HttpGet("summary")]
    [Authorize(Policy = PermissionCodes.CashFlowsRead)]
    public async Task<ActionResult<CashFlowSummaryDto>> GetSummary([FromQuery] Guid cashBoxId)
    {
        var summary = await _cashFlowService.GetSummaryByCashBoxIdAsync(cashBoxId);
        return Ok(summary);
    }

    [HttpPost]
    [Authorize(Policy = PermissionCodes.CashFlowsCreate)]
    public async Task<ActionResult<CashFlowResponseDto>> Create([FromBody] CashFlowCreateDto dto)
    {
        var userId = GetUserId();
        var created = await _cashFlowService.CreateAsync(dto, userId);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id:guid}")]
    [Authorize(Policy = PermissionCodes.CashFlowsCreate)]
    public async Task<ActionResult<CashFlowResponseDto>> Update(Guid id, [FromBody] CashFlowUpdateDto dto)
    {
        var userId = GetUserId();
        if (dto.Id == Guid.Empty)
            dto.Id = id;

        if (dto.Id != id)
            return BadRequest("El id del path no coincide con el id del body.");

        var updated = await _cashFlowService.UpdateAsync(dto, userId);
        return Ok(updated);
    }

    [HttpDelete("{id:guid}")]
    [Authorize(Policy = PermissionCodes.CashFlowsCreate)]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _cashFlowService.DeleteAsync(id);
        return NoContent();
    }

    private Guid GetUserId()
    {
        var claim = User.FindFirst("userId")?.Value;
        if (string.IsNullOrWhiteSpace(claim) || !Guid.TryParse(claim, out var userId))
            throw new UnauthorizedAccessException("Usuario no autenticado.");
        return userId;
    }
}
