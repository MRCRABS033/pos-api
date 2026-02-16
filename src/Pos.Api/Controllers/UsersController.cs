using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pos.Application.Dtos.Users;
using Pos.Application.Interfaces.Services;
using Pos.Domain.Security;

namespace Pos.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;

    public UsersController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet("{id:guid}")]
    [Authorize(Policy = PermissionCodes.UsersRead)]
    public async Task<ActionResult<UserResponseDto>> GetById(Guid id)
    {
        var user = await _userService.GetByIdAsync(id);
        return Ok(user);
    }

    [HttpGet("{id:guid}/permissions")]
    [Authorize(Policy = PermissionCodes.PermissionsRead)]
    public async Task<ActionResult<UserPermissionsResponseDto>> GetPermissions(Guid id)
    {
        var permissions = await _userService.GetPermissionsAsync(id);
        return Ok(permissions);
    }

    [HttpPut("{id:guid}/permissions")]
    [Authorize(Policy = "CanManagePermissions")]
    public async Task<ActionResult<UserPermissionsResponseDto>> UpdatePermissions(
        Guid id,
        [FromBody] UserPermissionsUpdateDto dto)
    {
        var actorUserId = GetUserId();
        var permissions = await _userService.UpdatePermissionsAsync(actorUserId, id, dto);
        return Ok(permissions);
    }

    [HttpGet]
    [Authorize(Policy = PermissionCodes.UsersRead)]
    public async Task<ActionResult<IReadOnlyList<UserResponseDto>>> GetAll(
        [FromQuery] string? email,
        [FromQuery] string? normaliceName,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 50)
    {
        if (!string.IsNullOrWhiteSpace(email))
        {
            var user = await _userService.GetByEmailAsync(email);
            if (user == null)
                return NotFound("Usuario no encontrado.");
            return Ok(user);
        }

        if (!string.IsNullOrWhiteSpace(normaliceName))
        {
            var user = await _userService.GetByNormaliceNameAsync(normaliceName);
            if (user == null)
                return NotFound("Usuario no encontrado.");
            return Ok(user);
        }

        var users = await _userService.GetAllAsync(page, pageSize);
        return Ok(users);
    }

    [HttpPost]
    [Authorize(Policy = PermissionCodes.UsersCreate)]
    public async Task<ActionResult<UserResponseDto>> Create([FromBody] UserCreateDto dto)
    {
        var created = await _userService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id:guid}")]
    [Authorize(Policy = PermissionCodes.UsersUpdate)]
    public async Task<ActionResult<UserResponseDto>> Update(Guid id, [FromBody] UserUpdateDto dto)
    {
        if (dto.Id == Guid.Empty)
            dto.Id = id;

        if (dto.Id != id)
            return BadRequest("El id del path no coincide con el id del body.");

        var updated = await _userService.UpdateAsync(dto);
        return Ok(updated);
    }

    [HttpDelete("{id:guid}")]
    [Authorize(Policy = PermissionCodes.UsersDelete)]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _userService.DeleteAsync(id);
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
