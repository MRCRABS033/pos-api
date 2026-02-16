using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pos.Application.Dtos.CashBoxes;
using Pos.Application.Interfaces.Services;
using Pos.Domain.Security;

namespace Pos.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CashBoxesController : ControllerBase
{
    private readonly ICashBoxService _cashBoxService;

    public CashBoxesController(ICashBoxService cashBoxService)
    {
        _cashBoxService = cashBoxService;
    }

    [HttpGet("{id:guid}")]
    [Authorize(Policy = PermissionCodes.CashBoxRead)]
    public async Task<ActionResult<CashBoxResponseDto>> GetById(Guid id)
    {
        var cashBox = await _cashBoxService.GetByIdAsync(id);
        return Ok(cashBox);
    }

    [HttpGet("by-date")]
    [Authorize(Policy = PermissionCodes.CashBoxRead)]
    public async Task<ActionResult<IReadOnlyList<CashBoxResponseDto>>> GetByDate([FromQuery] DateTime date)
    {
        var cashBoxes = await _cashBoxService.GetByDateAsync(date);
        return Ok(cashBoxes);
    }

    [HttpGet("latest")]
    [Authorize(Policy = PermissionCodes.CashBoxRead)]
    public async Task<ActionResult<CashBoxResponseDto>> GetLatest([FromQuery] Guid userId)
    {
        var cashBox = await _cashBoxService.GetLatestByUserIdAsync(userId);
        return Ok(cashBox);
    }

    [HttpGet]
    [Authorize(Policy = PermissionCodes.CashBoxRead)]
    public async Task<ActionResult<IReadOnlyList<CashBoxResponseDto>>> GetAll(
        [FromQuery] Guid? userId,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 50)
    {
        if (userId.HasValue)
        {
            var byUser = await _cashBoxService.GetByUserIdAsync(userId.Value);
            return Ok(byUser);
        }

        var cashBoxes = await _cashBoxService.GetAllAsync(page, pageSize);
        return Ok(cashBoxes);
    }

    [HttpGet("{cashBoxId:guid}/employees/{userId:guid}/summary")]
    [Authorize(Policy = PermissionCodes.UserActivityRead)]
    public async Task<ActionResult<EmployeeShiftSummaryDto>> GetEmployeeShiftSummary(Guid cashBoxId, Guid userId)
    {
        var summary = await _cashBoxService.GetEmployeeShiftSummaryAsync(cashBoxId, userId);
        return Ok(summary);
    }

    [HttpGet("{cashBoxId:guid}/summary")]
    [Authorize(Policy = PermissionCodes.ReportsCashCutsRead)]
    public async Task<ActionResult<ShiftCutSummaryDto>> GetShiftCutSummary(Guid cashBoxId)
    {
        var summary = await _cashBoxService.GetShiftCutSummaryAsync(cashBoxId);
        return Ok(summary);
    }

    [HttpPost]
    [Authorize(Policy = PermissionCodes.CashBoxOpen)]
    public async Task<ActionResult<CashBoxResponseDto>> Create([FromBody] CashBoxCreateDto dto)
    {
        var userId = GetUserId();
        var created = await _cashBoxService.CreateAsync(dto, userId);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id:guid}")]
    [Authorize(Policy = PermissionCodes.CashBoxClose)]
    public async Task<ActionResult<CashBoxResponseDto>> Update(Guid id, [FromBody] CashBoxUpdateDto dto)
    {
        if (dto.Id == Guid.Empty)
            dto.Id = id;

        if (dto.Id != id)
            return BadRequest("El id del path no coincide con el id del body.");

        var actorUserId = GetUserId();
        var updated = await _cashBoxService.UpdateAsync(dto, actorUserId);
        return Ok(updated);
    }

    [HttpDelete("{id:guid}")]
    [Authorize(Policy = PermissionCodes.CashBoxClose)]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _cashBoxService.DeleteAsync(id);
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
