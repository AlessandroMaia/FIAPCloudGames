using Application.Abstractions.Messaging;
using Application.Users.DTOs;

namespace Application.Users.Commands.Login;

public sealed record LoginUserCommand(string Email, string Password) : ICommand<CredentialsDto>;
