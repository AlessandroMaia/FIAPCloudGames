using Application.Abstractions.Messaging;
using Domain.Games;

namespace Application.Games.Commands.Update;

public sealed record UpdateGameCommand(
    Guid GameId,
    string? Title,
    string? Description,
    decimal? BasePrice,
    int? AgeRating,
    GameGenre? Genre) : ICommand;
