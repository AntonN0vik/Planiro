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

public record CreateTeamRequest(
    [Required] string Username);

public record JoinTeamRequest(
    [Required, MaxLength(8)] string Code,
    [Required] string Username);

public record TaskShema(
    [Required] string Id,
    [Required] string Title, 
    string Description,
    [Required] string Status,
    [Required] string Assignee,
    [Required] string Deadline
);

public record ReorderRequest(
    [Required] List<TaskShema> Task
);

public record TaskRequest(
    [Required] string Title,
    [Required] string Description,
    [Required] string Assignee,
    [Required] string Deadline,
    [Required] string Status
);

public record Member(
    [Required] string Id,
    [Required] string Name
);