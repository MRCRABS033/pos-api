using Pos.Application.Dtos.Users;
using Pos.Application.Interfaces.Services;
using Pos.Application.UseCases.Users;

namespace Pos.Application.Services.Users;

public class UserService : IUserService
{
    private readonly CreateUserUseCase _create;
    private readonly UpdateUserUseCase _update;
    private readonly GetUserByIdUseCase _getById;
    private readonly GetUserByEmailUseCase _getByEmail;
    private readonly GetUserByNormaliceNameUseCase _getByNormaliceName;
    private readonly GetAllUsersUseCase _getAll;
    private readonly DeleteUserUseCase _delete;

    public UserService(
        CreateUserUseCase create,
        UpdateUserUseCase update,
        GetUserByIdUseCase getById,
        GetUserByEmailUseCase getByEmail,
        GetUserByNormaliceNameUseCase getByNormaliceName,
        GetAllUsersUseCase getAll,
        DeleteUserUseCase delete)
    {
        _create = create;
        _update = update;
        _getById = getById;
        _getByEmail = getByEmail;
        _getByNormaliceName = getByNormaliceName;
        _getAll = getAll;
        _delete = delete;
    }

    public Task<UserResponseDto> CreateAsync(UserCreateDto dto)
    {
        return _create.ExecuteAsync(dto);
    }

    public Task<UserResponseDto> UpdateAsync(UserUpdateDto dto)
    {
        return _update.ExecuteAsync(dto);
    }

    public Task<UserResponseDto> GetByIdAsync(Guid id)
    {
        return _getById.ExecuteAsync(id);
    }

    public Task<UserResponseDto?> GetByEmailAsync(string email)
    {
        return _getByEmail.ExecuteAsync(email);
    }

    public Task<UserResponseDto?> GetByNormaliceNameAsync(string normaliceName)
    {
        return _getByNormaliceName.ExecuteAsync(normaliceName);
    }

    public Task<IReadOnlyList<UserResponseDto>> GetAllAsync()
    {
        return _getAll.ExecuteAsync();
    }

    public Task DeleteAsync(Guid id)
    {
        return _delete.ExecuteAsync(id);
    }
}
