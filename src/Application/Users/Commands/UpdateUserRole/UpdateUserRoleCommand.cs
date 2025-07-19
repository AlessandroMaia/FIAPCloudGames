using Application.Abstractions.Messaging;

namespace Application.Users.Commands.UpdateUserRole;

public sealed record UpdateUserRoleCommand(Guid UserId, string NewRole)
    : ICommand;