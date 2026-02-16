using Pos.Application.Dtos.Reports;
using Pos.Domain.Interfaces.Repositories;

namespace Pos.Application.UseCases.Reports;

public class GetSalesByDepartmentByCashBoxIdUseCase
{
    private readonly ICashBoxRepository _cashBoxRepository;
    private readonly ISaleRepository _saleRepository;

    public GetSalesByDepartmentByCashBoxIdUseCase(
        ICashBoxRepository cashBoxRepository,
        ISaleRepository saleRepository)
    {
        _cashBoxRepository = cashBoxRepository;
        _saleRepository = saleRepository;
    }

    public async Task<IReadOnlyList<SalesByDepartmentDto>> ExecuteAsync(Guid cashBoxId)
    {
        if (cashBoxId == Guid.Empty)
            throw new ArgumentException("El id de caja es requerido.", nameof(cashBoxId));

        _ = await _cashBoxRepository.GetByIdAsync(cashBoxId);

        var rows = await _saleRepository.GetSalesByDepartmentByCashBoxId(cashBoxId);
        return rows
            .Select(row => new SalesByDepartmentDto
            {
                DepartmentId = row.DepartmentId,
                DepartmentName = string.IsNullOrWhiteSpace(row.DepartmentName) ? "Sin categoría" : row.DepartmentName,
                TotalSales = row.TotalSales,
                ItemsCount = row.ItemsCount
            })
            .OrderByDescending(x => x.TotalSales)
            .ToList();
    }
}
