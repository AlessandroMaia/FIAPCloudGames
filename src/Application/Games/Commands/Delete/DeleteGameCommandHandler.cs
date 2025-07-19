using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Games;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Games.Commands.Delete;

internal sealed class DeleteGameCommandHandler(IApplicationDbContext context)
    : ICommandHandler<DeleteGameCommand>
{
    public async Task<Result> Handle(DeleteGameCommand command, CancellationToken cancellationToken)
    {
        var game  = await context.Games
            .SingleOrDefaultAsync(g => g.Id == command.GameId, cancellationToken);

        if(game is null)
            return Result.Failure(GameErrors.NotFound(command.GameId));

        game.IsActive = false;
        game.UpdatedAtUtc = DateTime.UtcNow;

        context.Games.Update(game);

        await context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
