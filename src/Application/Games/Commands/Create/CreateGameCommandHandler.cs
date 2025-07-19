using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Games;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Games.Commands.Create;

internal sealed class CreateGameCommandHandler(IApplicationDbContext context)
    : ICommandHandler<CreateGameCommand, Guid>
{
    public async Task<Result<Guid>> Handle(CreateGameCommand command, CancellationToken cancellationToken)
    {
        if (await context.Games.AnyAsync(u => u.Title == command.Title, cancellationToken))
        {
            return Result.Failure<Guid>(GameErrors.TitleNotUnique);
        }

        var game = new Game
        {
            Title = command.Title,
            Description = command.Description,  
            BasePrice = command.BasePrice,
            AgeRating = command.AgeRating,
            Genre = command.Genre
        };

        await context.Games.AddAsync(game, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        return game.Id;
    }
}
