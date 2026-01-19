using Pos.Application.Dtos.Users;
using Pos.Domain.Entities;
using Pos.Domain.Interfaces.Repositories;

namespace Pos.Application.UseCases.Users;

public class GetUserByEmailUseCase
{
    private readonly IUserRepository _userRepository;

    public GetUserByEmailUseCase(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<UserResponseDto?> ExecuteAsync(string email)
    {
        var user = await _userRepository.GetByEmail(email);
        return user == null ? null : Map(user);
    }

    private static UserResponseDto Map(User user)
    {
        return new UserResponseDto
        {
            Id = user.Id,
            Name = user.Name,
            LastName = user.LastName,
            NormaliceName = user.NormaliceName,
            Email = user.Email,
            Phone = user.Phone,
            Role = user.Role,
            IsActive = user.IsActive,
            Created = user.Created,
            Modified = user.Modified
        };
    }
}
