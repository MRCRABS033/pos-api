using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pos.Application.Dtos.SaleItems;
using Pos.Application.Interfaces.Services;

namespace Pos.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class SaleItemsController : ControllerBase
{
    private readonly ISaleItemService _saleItemService;

    public SaleItemsController(ISaleItemService saleItemService)
    {
        _saleItemService = saleItemService;
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<SaleItemResponseDto>> GetById(Guid id)
    {
        try
        {
            var saleItem = await _saleItemService.GetByIdAsync(id);
            return Ok(saleItem);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<SaleItemResponseDto>>> GetAll(
        [FromQuery] Guid? saleId,
        [FromQuery] Guid? productId)
    {
        if (saleId.HasValue)
        {
            var bySale = await _saleItemService.GetBySaleIdAsync(saleId.Value);
            return Ok(bySale);
        }

        if (productId.HasValue)
        {
            var byProduct = await _saleItemService.GetByProductIdAsync(productId.Value);
            return Ok(byProduct);
        }

        var items = await _saleItemService.GetAllAsync();
        return Ok(items);
    }

    [HttpPost]
    public async Task<ActionResult<SaleItemResponseDto>> Create(
        [FromQuery] Guid saleId,
        [FromBody] SaleItemCreateDto dto)
    {
        try
        {
            var created = await _saleItemService.CreateAsync(dto, saleId);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }
        catch (ArgumentNullException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<SaleItemResponseDto>> Update(Guid id, [FromBody] SaleItemUpdateDto dto)
    {
        if (dto.Id == Guid.Empty)
            dto.Id = id;

        if (dto.Id != id)
            return BadRequest("El id del path no coincide con el id del body.");

        try
        {
            var updated = await _saleItemService.UpdateAsync(dto);
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
            await _saleItemService.DeleteAsync(id);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }
}
