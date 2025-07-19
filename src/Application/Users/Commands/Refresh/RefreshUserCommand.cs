using Application.Abstractions.Messaging;
using Application.Users.DTOs;

namespace Application.Users.Commands.Refresh;

public sealed record RefreshUserCommand(string RefreshToken) : ICommand<CredentialsDto>;
