using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Planiro.Application.Services;
using Planiro.Models;

namespace Planiro.Controllers;
public class CreateTeamRequest
{
    public string Username { get; set; }
}

public class JoinRequest
{
    public string Code { get; set; }
    public string Username { get; set; }
}