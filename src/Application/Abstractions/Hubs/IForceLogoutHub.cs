namespace Application.Abstractions.Hubs;

public interface IForceLogoutHub
{
    Task ForceLogout(Guid UserId);
}
