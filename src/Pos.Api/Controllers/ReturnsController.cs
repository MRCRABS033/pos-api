using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pos.Application.Dtos.Returns;
using Pos.Application.Interfaces.Services;

namespace Pos.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ReturnsController : ControllerBase
{
    private readonly IReturnService _returnService;

    public ReturnsController(IReturnService returnService)
    {
        _returnService = returnService;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<ReturnResponseDto>>> GetAll([FromQuery] Guid? cashBoxId)
    {
        if (cashBoxId.HasValue)
        {
            var list = await _returnService.GetByCashBoxIdAsync(cashBoxId.Value);
            return Ok(list);
        }

        return Ok(Array.Empty<ReturnResponseDto>());
    }

    [HttpGet("summary")]
    public async Task<ActionResult<ReturnSummaryDto>> GetSummary([FromQuery] Guid cashBoxId)
    {
        var summary = await _returnService.GetSummaryByCashBoxIdAsync(cashBoxId);
        return Ok(summary);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ReturnResponseDto>> GetById(Guid id)
    {
        var ret = await _returnService.GetByIdAsync(id);
        return Ok(ret);
    }
}
