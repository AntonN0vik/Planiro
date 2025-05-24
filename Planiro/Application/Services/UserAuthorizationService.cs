using Planiro.Domain.Entities;
using Planiro.Domain.IRepositories;

namespace Planiro.Application.Services;

using Planiro.Models;
using Planiro.Domain;

public static class UserAuthorizationService
{
    public static void RegisterUser(IUserRepository userRepository, RegisterRequest registerRequest)
    {
        var username = registerRequest.Username;
        var firstName = registerRequest.FirstName;
        var lastName = registerRequest.LastName;
        var tempPassword = registerRequest.Password;

        var passwordHashed = PasswordHasher.HashPassword(tempPassword);

        if (userRepository.IsUsernameExist(username))
            throw new ArgumentException("Username already exist");

        var userId = Guid.NewGuid();
        var planerId = Guid.NewGuid();
        userRepository.SaveUser(new User(userId, firstName, lastName, username), passwordHashed);
    }

    public static void AuthorizeUser(IUserRepository userRepository,
        LoginRequest registerRequest)
    {
        var tempUsername = registerRequest.Username;
        var password = registerRequest.Password;

        if (!userRepository.CheckPassword(tempUsername, password))
            throw new ArgumentException("Invalid username or password");
    }
}