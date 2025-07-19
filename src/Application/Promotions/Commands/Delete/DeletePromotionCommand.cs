using Application.Abstractions.Messaging;

namespace Application.Promotions.Commands.Delete;

public sealed record DeletePromotionCommand(Guid PromotionId) : ICommand;
