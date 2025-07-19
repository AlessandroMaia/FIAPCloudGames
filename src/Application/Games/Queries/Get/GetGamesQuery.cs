using Application.Abstractions.Messaging;
using Application.Games.DTOs;
using SharedKernel;

namespace Application.Games.Queries.Get;

public sealed record GetGamesQuery(
    string? Title,
    decimal? InitialBasePrice,
    decimal? FinalBasePrice,
    int? AgeRating,
    string? Genre,
    bool? IsOnPromotion,
    int? PageNumber = 1,
    int PageSize = 10) : IQuery<PagedResponse<GameSummaryDto>>;
