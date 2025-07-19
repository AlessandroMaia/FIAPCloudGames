using Application.Abstractions.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace Web.Api.Hubs;

public sealed class ForceLogoutHub : Hub<IForceLogoutHub> { }
