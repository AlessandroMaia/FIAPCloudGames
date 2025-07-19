using SharedKernel;

namespace Domain.Roles;

public static class RolesErrors
{
    public static Error NotFound(string name) => Error.NotFound(
        $"The role with the name = '{name}' was not found");
}
