using Application.Abstractions.Messaging;

namespace Application.Users.Commands.Logout;

public sealed record LogoutUserCommand(string? Token, string? RefreshToken) : ICommand;
