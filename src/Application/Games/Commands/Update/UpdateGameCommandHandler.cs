using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Games;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Games.Commands.Update;

internal sealed class UpdateGameCommandHandler(IApplicationDbContext context)
    : ICommandHandler<UpdateGameCommand>
{
    public async Task<Result> Handle(UpdateGameCommand command, CancellationToken cancellationToken)
    {
        var game = await context.Games
            .FirstOrDefaultAsync(g => g.Id == command.GameId, cancellationToken);

        if (game is null)
            return Result.Failure(GameErrors.NotFound(command.GameId));

        if (command.Title is not null)
            game.Title = command.Title;

        if (command.Description is not null)
            game.Description = command.Description;

        if (command.BasePrice is not null)
            game.BasePrice = command.BasePrice.Value;

        if (command.AgeRating is not null)
            game.AgeRating = command.AgeRating.Value;

        if (command.Genre is not null)
            game.Genre = command.Genre.Value;

        game.UpdatedAtUtc = DateTime.UtcNow;

        context.Games.Update(game);

        await context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
