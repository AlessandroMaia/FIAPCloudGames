using Application.Abstractions.Messaging;

namespace Application.Users.Commands.ForceLogout;

public sealed record ForceLogoutUserCommand(Guid UserId) : ICommand;
