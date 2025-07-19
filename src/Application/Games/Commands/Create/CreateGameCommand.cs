using Application.Abstractions.Messaging;
using Domain.Games;

namespace Application.Games.Commands.Create;

public sealed record CreateGameCommand(
    string Title, 
    string? Description, 
    decimal BasePrice, 
    int AgeRating, 
    GameGenre Genre) 
        : ICommand<Guid>;
