using Application.Abstractions.Messaging;
using Domain.PurchaseHistories;

namespace Application.Purchases.Commands.PurchaseGame;

public sealed record PurchaseGameCommand(Guid GameId, Guid? PromotionId, PaymentMethod PaymentMethod) : ICommand; 