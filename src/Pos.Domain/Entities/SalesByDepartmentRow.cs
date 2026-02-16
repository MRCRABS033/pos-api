namespace Pos.Domain.Entities;

public record SalesByDepartmentRow(
    Guid? DepartmentId,
    string? DepartmentName,
    decimal TotalSales,
    int ItemsCount);
