using Application.Abstractions.Messaging;
using Application.Users.DTOs;

namespace Application.Users.Queries.GetMe;

public sealed record GetMeQuery() : IQuery<UserDetailDto>;
