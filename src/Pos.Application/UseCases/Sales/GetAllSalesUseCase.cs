using Pos.Application.Dtos.SaleItems;
using Pos.Application.Dtos.Sales;
using Pos.Domain.Entities;
using Pos.Domain.Interfaces.Repositories;

namespace Pos.Application.UseCases.Sales;

public class GetAllSalesUseCase
{
    private readonly ISaleRepository _saleRepository;

    public GetAllSalesUseCase(ISaleRepository saleRepository)
    {
        _saleRepository = saleRepository;
    }

    public async Task<IReadOnlyList<SaleResponseDto>> ExecuteAsync(int page = 1, int pageSize = 50)
    {
        var sales = await _saleRepository.GetAllAsync(page, pageSize);
        return sales.Select(Map).ToList();
    }

    private static SaleResponseDto Map(Sale sale)
    {
        return new SaleResponseDto
        {
            Id = sale.Id,
            UserId = sale.UserId,
            CashBoxId = sale.CashBoxId,
            PaymentType = sale.PaymentType,
            Total = sale.Total,
            Discount = sale.Discount,
            ItemsCount = 0,
            CreatedAt = sale.CreatedAt,
            UpdatedAt = sale.UpdatedAt,
            Items = new List<SaleItemResponseDto>()
        };
    }
}
