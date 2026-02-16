using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pos.Application.Dtos.Products;
using Pos.Application.Interfaces.Services;
using Pos.Domain.Security;

namespace Pos.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ProductsController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductsController(IProductService productService)
    {
        _productService = productService;
    }

    [HttpGet("{id:guid}")]
    [Authorize(Policy = PermissionCodes.ProductsRead)]
    public async Task<ActionResult<ProductResponseDto>> GetById(Guid id)
    {
        var product = await _productService.GetByIdAsync(id);
        return Ok(product);
    }

    [HttpGet("sku/{sku}")]
    [Authorize(Policy = PermissionCodes.ProductsRead)]
    public async Task<ActionResult<ProductResponseDto>> GetBySku(string sku)
    {
        var product = await _productService.GetBySkuAsync(sku);
        return Ok(product);
    }

    [HttpGet]
    [Authorize(Policy = PermissionCodes.ProductsRead)]
    public async Task<ActionResult<IReadOnlyList<ProductResponseDto>>> GetAll(
        [FromQuery] string? term,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 50)
    {
        if (!string.IsNullOrWhiteSpace(term))
        {
            var results = await _productService.SearchAsync(term);
            return Ok(results);
        }

        var products = await _productService.GetAllAsync(page, pageSize);
        return Ok(products);
    }

    [HttpGet("category/{categoryId:guid}")]
    [Authorize(Policy = PermissionCodes.ProductsRead)]
    public async Task<ActionResult<IReadOnlyList<ProductResponseDto>>> GetByCategory(Guid categoryId)
    {
        var products = await _productService.GetByCategoryIdAsync(categoryId);
        return Ok(products);
    }

    [HttpPost]
    [Authorize(Policy = PermissionCodes.ProductsCreate)]
    public async Task<ActionResult<ProductResponseDto>> Create([FromBody] ProductCreateDto dto)
    {
        var userId = GetUserId();
        var created = await _productService.CreateAsync(dto, userId);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id:guid}")]
    [Authorize(Policy = PermissionCodes.ProductsUpdate)]
    public async Task<ActionResult<ProductResponseDto>> Update(Guid id, [FromBody] ProductUpdateDto dto)
    {
        var userId = GetUserId();
        if (dto.Id == Guid.Empty)
            dto.Id = id;

        if (dto.Id != id)
            return BadRequest("El id del path no coincide con el id del body.");

        var updated = await _productService.UpdateAsync(dto, userId);
        return Ok(updated);
    }

    [HttpDelete("{id:guid}")]
    [Authorize(Policy = PermissionCodes.ProductsDelete)]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _productService.DeleteAsync(id);
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
