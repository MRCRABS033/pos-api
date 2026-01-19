using Pos.Domain.Interfaces.Repositories;

namespace Pos.Application.UseCases.Users;

public class DeleteUserUseCase
{
    private readonly IUserRepository _userRepository;

    public DeleteUserUseCase(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public Task ExecuteAsync(Guid id)
    {
        return _userRepository.DeleteAsync(id);
    }
}
