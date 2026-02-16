namespace Pos.Domain.Security;

public static class PermissionCodes
{
    public const string ProductsRead = "products.read";
    public const string ProductsCreate = "products.create";
    public const string ProductsUpdate = "products.update";
    public const string ProductsDelete = "products.delete";

    public const string SalesRead = "sales.read";
    public const string SalesCreate = "sales.create";

    public const string ReturnsRead = "returns.read";
    public const string ReturnsCreate = "returns.create";

    public const string CashBoxRead = "cashbox.read";
    public const string CashBoxOpen = "cashbox.open";
    public const string CashBoxClose = "cashbox.close";

    public const string CashFlowsRead = "cashflows.read";
    public const string CashFlowsCreate = "cashflows.create";

    public const string UsersRead = "users.read";
    public const string UsersCreate = "users.create";
    public const string UsersUpdate = "users.update";
    public const string UsersDelete = "users.delete";

    public const string PermissionsRead = "permissions.read";
    public const string PermissionsManage = "permissions.manage";

    public const string UserActivityRead = "useractivity.read";
    public const string ReportsProfitsRead = "reports.profits.read";
    public const string ReportsCashCutsRead = "reports.cashcuts.read";

    public static IReadOnlyList<string> All { get; } =
    [
        ProductsRead,
        ProductsCreate,
        ProductsUpdate,
        ProductsDelete,
        SalesRead,
        SalesCreate,
        ReturnsRead,
        ReturnsCreate,
        CashBoxRead,
        CashBoxOpen,
        CashBoxClose,
        CashFlowsRead,
        CashFlowsCreate,
        UsersRead,
        UsersCreate,
        UsersUpdate,
        UsersDelete,
        PermissionsRead,
        PermissionsManage,
        UserActivityRead,
        ReportsProfitsRead,
        ReportsCashCutsRead
    ];
}
