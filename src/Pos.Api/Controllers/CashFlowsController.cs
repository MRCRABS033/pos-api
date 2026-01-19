using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pos.Application.Dtos.CashFlows;
using Pos.Application.Interfaces.Services;

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
    public async Task<ActionResult<CashFlowResponseDto>> GetById(Guid id)
    {
        try
        {
            var cashFlow = await _cashFlowService.GetByIdAsync(id);
            return Ok(cashFlow);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<CashFlowResponseDto>>> GetAll(
        [FromQuery] Guid? userId,
        [FromQuery] DateTime? startDate,
        [FromQuery] DateTime? endDate)
    {
        if (userId.HasValue)
        {
            var byUser = await _cashFlowService.GetByUserIdAsync(userId.Value);
            return Ok(byUser);
        }

        if (startDate.HasValue && endDate.HasValue)
        {
            var byDate = await _cashFlowService.GetByDateRangeAsync(startDate.Value, endDate.Value);
            return Ok(byDate);
        }

        var cashFlows = await _cashFlowService.GetAllAsync();
        return Ok(cashFlows);
    }

    [HttpPost]
    public async Task<ActionResult<CashFlowResponseDto>> Create([FromBody] CashFlowCreateDto dto)
    {
        try
        {
            var created = await _cashFlowService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }
        catch (ArgumentNullException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<CashFlowResponseDto>> Update(Guid id, [FromBody] CashFlowUpdateDto dto)
    {
        if (dto.Id == Guid.Empty)
            dto.Id = id;

        if (dto.Id != id)
            return BadRequest("El id del path no coincide con el id del body.");

        try
        {
            var updated = await _cashFlowService.UpdateAsync(dto);
            return Ok(updated);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (ArgumentNullException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        try
        {
            await _cashFlowService.DeleteAsync(id);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }
}
