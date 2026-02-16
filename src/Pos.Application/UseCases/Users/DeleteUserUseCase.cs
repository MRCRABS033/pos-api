using Pos.Domain.Interfaces.Repositories;

namespace Pos.Application.UseCases.Users;

public class DeleteUserUseCase
{
    private readonly IUserRepository _userRepository;

    public DeleteUserUseCase(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task ExecuteAsync(Guid id)
    {
        var user = await _userRepository.GetByIdAsync(id);
        if (user.IsOwner)
            throw new InvalidOperationException("No se puede eliminar al usuario owner.");

        await _userRepository.DeleteAsync(id);
    }
}
