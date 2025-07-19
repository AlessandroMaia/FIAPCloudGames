using Application.Abstractions.Messaging;

namespace Application.Users.Commands.Register;

public sealed record RegisterUserCommand(string Email, string Name, string Password)
    : ICommand<Guid>;
