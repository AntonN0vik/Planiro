using Planiro.Domain.Entities;
using Planiro.Domain.IRepositories;

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

    public void RegisterUser(RegisterRequest registerRequest)
    {
        var passwordHashed = _passwordHasher.HashPassword(registerRequest.Password);
        
        if (!_userRepository.IsUsernameExistAsync(registerRequest.Username).Result)
            throw new ArgumentException("Username already exist");

        var userId = Guid.NewGuid();
        _userRepository.SaveUserAsync(new User(userId, 
                registerRequest.FirstName,
                registerRequest.LastName,
                registerRequest.Username, 
                new List<Guid>(), new List<Guid>()), 
            passwordHashed);
    }

    public  void AuthorizeUser(LoginRequest registerRequest)
    {
        var tempUsername = registerRequest.Username;
        var password = registerRequest.Password;

        if (!_userRepository.CheckPasswordAsync(tempUsername, password).Result)
            throw new ArgumentException("Invalid username or password");
    }
}