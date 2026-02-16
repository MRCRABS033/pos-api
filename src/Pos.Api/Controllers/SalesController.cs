using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pos.Application.Dtos.Sales;
using Pos.Application.Dtos.Returns;
using Pos.Application.Interfaces.Services;

namespace Pos.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class SalesController : ControllerBase
{
    private readonly ISaleService _saleService;
    private readonly IReturnService _returnService;

    public SalesController(ISaleService saleService, IReturnService returnService)
    {
        _saleService = saleService;
        _returnService = returnService;
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<SaleResponseDto>> GetById(Guid id)
    {
        var sale = await _saleService.GetByIdAsync(id);
        return Ok(sale);
    }

    [HttpPost("{saleId:guid}/returns")]
    public async Task<ActionResult<ReturnResponseDto>> CreateReturn(Guid saleId, [FromBody] ReturnCreateDto dto)
    {
        var userId = GetUserId();
        var created = await _returnService.CreateAsync(saleId, dto, userId);
        return Ok(created);
    }

    [HttpGet("{saleId:guid}/returns")]
    public async Task<ActionResult<IReadOnlyList<ReturnResponseDto>>> GetReturns(Guid saleId)
    {
        var returns = await _returnService.GetBySaleIdAsync(saleId);
        return Ok(returns);
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<SaleResponseDto>>> GetAll(
        [FromQuery] Guid? userId,
        [FromQuery] Guid? cashBoxId,
        [FromQuery] DateTime? startDate,
        [FromQuery] DateTime? endDate,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 50)
    {
        if (userId.HasValue)
        {
            var byUser = await _saleService.GetByUserIdAsync(userId.Value);
            return Ok(byUser);
        }

        if (cashBoxId.HasValue)
        {
            var byCashBox = await _saleService.GetByCashBoxIdAsync(cashBoxId.Value);
            return Ok(byCashBox);
        }

        if (startDate.HasValue && endDate.HasValue)
        {
            var byDate = await _saleService.GetByDateRangeAsync(startDate.Value, endDate.Value);
            return Ok(byDate);
        }

        var sales = await _saleService.GetAllAsync(page, pageSize);
        return Ok(sales);
    }

    [HttpPost]
    public async Task<ActionResult<SaleResponseDto>> Create([FromBody] SaleCreateDto dto)
    {
        var userId = GetUserId();
        var created = await _saleService.CreateAsync(dto, userId);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<SaleResponseDto>> Update(Guid id, [FromBody] SaleUpdateDto dto)
    {
        if (dto.Id == Guid.Empty)
            dto.Id = id;

        if (dto.Id != id)
            return BadRequest("El id del path no coincide con el id del body.");

        var userId = GetUserId();
        var updated = await _saleService.UpdateAsync(dto, userId);
        return Ok(updated);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _saleService.DeleteAsync(id);
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
