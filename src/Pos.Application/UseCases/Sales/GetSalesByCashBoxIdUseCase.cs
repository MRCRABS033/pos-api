using Pos.Application.Dtos.SaleItems;
using Pos.Application.Dtos.Sales;
using Pos.Domain.Entities;
using Pos.Domain.Interfaces.Repositories;

namespace Pos.Application.UseCases.Sales;

public class GetSalesByCashBoxIdUseCase
{
    private readonly ISaleRepository _saleRepository;

    public GetSalesByCashBoxIdUseCase(ISaleRepository saleRepository)
    {
        _saleRepository = saleRepository;
    }

    public async Task<IReadOnlyList<SaleResponseDto>> ExecuteAsync(Guid cashBoxId)
    {
        var sales = await _saleRepository.GetByCashBoxIdWithItemsCount(cashBoxId);
        return sales.Select(Map).ToList();
    }

    private static SaleResponseDto Map(SaleWithItemsCount summary)
    {
        var sale = summary.Sale;
        return new SaleResponseDto
        {
            Id = sale.Id,
            UserId = sale.UserId,
            CashBoxId = sale.CashBoxId,
            PaymentType = sale.PaymentType,
            Total = sale.Total,
            Discount = sale.Discount,
            ItemsCount = summary.ItemsCount,
            CreatedAt = sale.CreatedAt,
            UpdatedAt = sale.UpdatedAt,
            Items = new List<SaleItemResponseDto>()
        };
    }
}
