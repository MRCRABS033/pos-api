using Pos.Application.Dtos.Auth;

namespace Pos.Application.Interfaces.Services;

public interface IAuthService
{
    Task<AuthResponseDto> RegisterAsync(AuthRegisterDto dto);
    Task<AuthResponseDto> LoginAsync(AuthLoginDto dto);
}
