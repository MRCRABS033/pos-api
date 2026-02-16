using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pos.Application.Dtos.Reports;
using Pos.Application.UseCases.Reports;
using Pos.Domain.Security;

namespace Pos.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ReportsController : ControllerBase
{
    private readonly GetSalesByDepartmentByCashBoxIdUseCase _getSalesByDepartmentByCashBoxId;

    public ReportsController(GetSalesByDepartmentByCashBoxIdUseCase getSalesByDepartmentByCashBoxId)
    {
        _getSalesByDepartmentByCashBoxId = getSalesByDepartmentByCashBoxId;
    }

    [HttpGet("cashboxes/{cashBoxId}/sales-by-department")]
    [Authorize(Policy = PermissionCodes.ReportsProfitsRead)]
    public async Task<ActionResult<IReadOnlyList<SalesByDepartmentDto>>> GetSalesByDepartment(string cashBoxId)
    {
        if (!Guid.TryParse(cashBoxId, out var parsedCashBoxId))
            return BadRequest("El cashBoxId debe ser un GUID válido.");

        var report = await _getSalesByDepartmentByCashBoxId.ExecuteAsync(parsedCashBoxId);
        return Ok(report);
    }
}
