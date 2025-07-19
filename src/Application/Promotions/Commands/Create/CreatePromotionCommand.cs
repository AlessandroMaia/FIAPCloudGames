using Application.Abstractions.Messaging;

namespace Application.Promotions.Commands.Create;

public sealed record CreatePromotionCommand(
    string Name,
    decimal PercentageDiscount,
    DateTime StartDateUtc,
    DateTime EndDateUtc,
    Guid[] GameIds)
        : ICommand<Guid>;