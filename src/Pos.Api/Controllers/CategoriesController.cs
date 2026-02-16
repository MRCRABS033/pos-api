using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pos.Application.Dtos.Categories;
using Pos.Application.Interfaces.Services;

namespace Pos.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CategoriesController : ControllerBase
{
    private readonly ICategoryService _categoryService;

    public CategoriesController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<CategoryResponseDto>> GetById(Guid id)
    {
        var category = await _categoryService.GetByIdAsync(id);
        return Ok(category);
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<CategoryResponseDto>>> GetAll(
        [FromQuery] string? name,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 50)
    {
        if (!string.IsNullOrWhiteSpace(name))
        {
            var result = await _categoryService.GetByNameAsync(name);
            if (result == null)
                return NotFound("Categoria no encontrada.");
            return Ok(result);
        }

        var categories = await _categoryService.GetAllAsync(page, pageSize);
        return Ok(categories);
    }

    [HttpPost]
    public async Task<ActionResult<CategoryResponseDto>> Create([FromBody] CategoryCreateDto dto)
    {
        var created = await _categoryService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<CategoryResponseDto>> Update(Guid id, [FromBody] CategoryUpdateDto dto)
    {
        if (dto.Id == Guid.Empty)
            dto.Id = id;

        if (dto.Id != id)
            return BadRequest("El id del path no coincide con el id del body.");

        var updated = await _categoryService.UpdateAsync(dto);
        return Ok(updated);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _categoryService.DeleteAsync(id);
        return NoContent();
    }
}
