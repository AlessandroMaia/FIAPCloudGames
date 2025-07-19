using Application.Abstractions.Messaging;
using Application.Users.DTOs;
using SharedKernel;

namespace Application.Users.Queries.Get;

public sealed record GetUsersQuery(int? PageNumber = 1, int PageSize = 10) : IQuery<PagedResponse<UserSummaryDto>>;
