using Application.Abstractions.Messaging;
using Application.Users.DTOs;

namespace Application.Users.Queries.GetById;

public sealed record GetUserByIdQuery(Guid UserId) : IQuery<UserDetailDto>;
