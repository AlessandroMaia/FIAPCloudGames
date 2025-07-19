using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Users;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Users.Commands.Delete;

internal sealed class DeleteUserCommandHandler(
    IApplicationDbContext context)
        : ICommandHandler<DeleteUserCommand>
{
    public async Task<Result> Handle(DeleteUserCommand command, CancellationToken cancellationToken)
    {
        User? user = await context.Users
            .AsNoTracking()
            .SingleOrDefaultAsync(u => u.Id == command.Id, cancellationToken);

        if (user is null)
        {
            return Result.Failure<string>(UserErrors.NotFound(command.Id));
        }

        context.Users.Remove(user);

        await context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
