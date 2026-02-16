namespace Pos.Domain.Security;

public static class PermissionProfiles
{
    public static IReadOnlyList<string> GetDefaultForRole(string? role)
    {
        if (string.Equals(role, "Admin", StringComparison.OrdinalIgnoreCase))
            return PermissionCodes.All;

        return UserDefault;
    }

    public static IReadOnlyList<string> UserDefault { get; } =
    [
        PermissionCodes.ProductsRead,
        PermissionCodes.SalesRead,
        PermissionCodes.SalesCreate,
        PermissionCodes.ReturnsRead,
        PermissionCodes.ReturnsCreate,
        PermissionCodes.CashBoxRead,
        PermissionCodes.CashFlowsRead,
        PermissionCodes.CashFlowsCreate
    ];
}
