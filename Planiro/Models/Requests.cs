namespace Planiro.Models;
using System.ComponentModel.DataAnnotations;

public record RegisterRequest(
    [Required] string FirstName,
    [Required] string LastName,
    [Required, MinLength(3)] string Username,
    [Required, MinLength(6)] string Password);

public record LoginRequest(
    [Required] string Username,
    [Required] string Password);