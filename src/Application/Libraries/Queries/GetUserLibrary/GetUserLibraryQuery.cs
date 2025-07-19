using Application.Abstractions.Messaging;
using Application.Games.DTOs;
using SharedKernel;

namespace Application.Libraries.Queries.GetUserLibrary;

public sealed record GetUserLibraryQuery(Guid UserId, int? PageNumber = 1, int PageSize = 10) : IQuery<PagedResponse<GameMinimalDto>>;

