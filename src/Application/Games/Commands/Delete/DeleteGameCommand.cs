using Application.Abstractions.Messaging;

namespace Application.Games.Commands.Delete;

public sealed record DeleteGameCommand(Guid GameId) : ICommand;
