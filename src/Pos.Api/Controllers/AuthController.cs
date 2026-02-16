using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pos.Application.Dtos.Auth;
using Pos.Application.Interfaces.Services;

namespace Pos.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<ActionResult<AuthResponseDto>> Register([FromBody] AuthRegisterDto dto)
    {
        var result = await _authService.RegisterAsync(dto);
        return CreatedAtAction(nameof(Register), new { id = result.UserId }, result);
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<ActionResult<AuthResponseDto>> Login([FromBody] AuthLoginDto dto)
    {
        var result = await _authService.LoginAsync(dto);
        return Ok(result);
    }
}
