using Pos.Application.Dtos.SaleItems;
using Pos.Application.Dtos.Sales;
using Pos.Domain.Entities;
using Pos.Domain.Interfaces.Repositories;

namespace Pos.Application.UseCases.Sales;

public class GetSalesByDateRangeUseCase
{
    private readonly ISaleRepository _saleRepository;

    public GetSalesByDateRangeUseCase(ISaleRepository saleRepository)
    {
        _saleRepository = saleRepository;
    }

    public async Task<IReadOnlyList<SaleResponseDto>> ExecuteAsync(DateTime startDate, DateTime endDate)
    {
        var sales = await _saleRepository.GetByDateRange(startDate, endDate);
        return sales.Select(Map).ToList();
    }

    private static SaleResponseDto Map(Sale sale)
    {
        return new SaleResponseDto
        {
            Id = sale.Id,
            UserId = sale.UserId,
            CreatedAt = sale.CreatedAt,
            UpdatedAt = sale.UpdatedAt,
            Items = new List<SaleItemResponseDto>()
        };
    }
}
