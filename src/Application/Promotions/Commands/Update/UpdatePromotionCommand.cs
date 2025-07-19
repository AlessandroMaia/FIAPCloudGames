using Application.Abstractions.Messaging;

namespace Application.Promotions.Commands.Update;

public sealed record UpdatePromotionCommand(
    Guid Id,
    string? Name,
    decimal? PercentageDiscount,
    DateTime? StartDateUtc,
    DateTime? EndDateUtc,
    Guid[] GameIds) : ICommand;
