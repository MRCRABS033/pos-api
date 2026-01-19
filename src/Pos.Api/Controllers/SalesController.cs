using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pos.Application.Dtos.Sales;
using Pos.Application.Interfaces.Services;

namespace Pos.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class SalesController : ControllerBase
{
    private readonly ISaleService _saleService;

    public SalesController(ISaleService saleService)
    {
        _saleService = saleService;
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<SaleResponseDto>> GetById(Guid id)
    {
        try
        {
            var sale = await _saleService.GetByIdAsync(id);
            return Ok(sale);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<SaleResponseDto>>> GetAll(
        [FromQuery] Guid? userId,
        [FromQuery] DateTime? startDate,
        [FromQuery] DateTime? endDate)
    {
        if (userId.HasValue)
        {
            var byUser = await _saleService.GetByUserIdAsync(userId.Value);
            return Ok(byUser);
        }

        if (startDate.HasValue && endDate.HasValue)
        {
            var byDate = await _saleService.GetByDateRangeAsync(startDate.Value, endDate.Value);
            return Ok(byDate);
        }

        var sales = await _saleService.GetAllAsync();
        return Ok(sales);
    }

    [HttpPost]
    public async Task<ActionResult<SaleResponseDto>> Create([FromBody] SaleCreateDto dto)
    {
        try
        {
            var userId = GetUserId();
            var created = await _saleService.CreateAsync(dto, userId);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }
        catch (ArgumentNullException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<SaleResponseDto>> Update(Guid id, [FromBody] SaleUpdateDto dto)
    {
        if (dto.Id == Guid.Empty)
            dto.Id = id;

        if (dto.Id != id)
            return BadRequest("El id del path no coincide con el id del body.");

        try
        {
            var userId = GetUserId();
            var updated = await _saleService.UpdateAsync(dto, userId);
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
            await _saleService.DeleteAsync(id);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    private Guid GetUserId()
    {
        var claim = User.FindFirst("userId")?.Value;
        if (string.IsNullOrWhiteSpace(claim) || !Guid.TryParse(claim, out var userId))
            throw new UnauthorizedAccessException("Usuario no autenticado.");
        return userId;
    }
}
