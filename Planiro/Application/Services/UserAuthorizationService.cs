using Planiro.Domain.Entities;
using Planiro.Domain.IRepositories;
using Task = System.Threading.Tasks.Task;

namespace Planiro.Application.Services;

using Planiro.Models;
using Planiro.Domain;

public class UserAuthorizationService
{
    private readonly IUserRepository _userRepository;
    private readonly PasswordHasher _passwordHasher;

    public UserAuthorizationService(
        IUserRepository userRepository,
        PasswordHasher passwordHasher)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
    }

    public async Task RegisterUser(RegisterRequest registerRequest)
    {
        var passwordHashed = _passwordHasher.HashPassword(registerRequest.Password);
        var existingUser = await _userRepository.IsUsernameExistAsync(registerRequest.Username);
        if (!existingUser)
            throw new ArgumentException("Username already exist");

        var userId = Guid.NewGuid();
        await _userRepository.SaveUserAsync(new User(userId, 
                registerRequest.FirstName,
                registerRequest.LastName,
                registerRequest.Username, 
                new List<Guid>(), new List<Guid>()), 
            passwordHashed);
    }

    public async void AuthorizeUser(LoginRequest registerRequest)
    {
        var tempUsername = registerRequest.Username;
        var password = registerRequest.Password;
        
        var checkedPassword = await _userRepository.CheckPasswordAsync(tempUsername, password);
        if (!checkedPassword)
            throw new ArgumentException("Invalid username or password");
    }
}