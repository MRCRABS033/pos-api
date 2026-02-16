using Pos.Application.Dtos.Returns;
using Pos.Domain.Entities;
using Pos.Domain.Interfaces.Repositories;

namespace Pos.Application.UseCases.Returns;

public class GetReturnByIdUseCase
{
    private readonly IReturnRepository _returnRepository;

    public GetReturnByIdUseCase(IReturnRepository returnRepository)
    {
        _returnRepository = returnRepository;
    }

    public async Task<ReturnResponseDto> ExecuteAsync(Guid id)
    {
        var ret = await _returnRepository.GetByIdAsync(id);
        return Map(ret);
    }

    private static ReturnResponseDto Map(Return ret)
    {
        return new ReturnResponseDto
        {
            Id = ret.Id,
            SaleId = ret.SaleId,
            UserId = ret.UserId,
            CreatedAt = ret.CreatedAt,
            Reason = ret.Reason,
            Total = ret.Total,
            PaymentType = ret.PaymentType,
            Items = ret.Items.Select(MapItem).ToList()
        };
    }

    private static ReturnItemResponseDto MapItem(ReturnItem item)
    {
        return new ReturnItemResponseDto
        {
            Id = item.Id,
            ReturnId = item.ReturnId,
            SaleItemId = item.SaleItemId,
            ProductId = item.ProductId,
            ProductName = item.Product?.Name,
            Quantity = item.Quantity,
            UnitPrice = item.UnitPrice
        };
    }
}
