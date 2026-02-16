namespace Pos.Application.Dtos.Reports;

public class SalesByDepartmentDto
{
    public Guid? DepartmentId { get; set; }
    public string DepartmentName { get; set; } = string.Empty;
    public decimal TotalSales { get; set; }
    public int ItemsCount { get; set; }
}
