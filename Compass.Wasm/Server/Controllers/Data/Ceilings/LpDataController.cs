using System.ComponentModel.DataAnnotations;
using Compass.DataService.Infrastructure;
using Compass.Dtos;
using Compass.Wasm.Server.Services.Data.Ceilings;
using Compass.Wasm.Shared.Data.Ceilings;

namespace Compass.Wasm.Server.Controllers.Data.Ceilings;

[Route("api/[controller]")]
[ApiController]
[UnitOfWork(typeof(DataDbContext))]
public class LpDataController : ControllerBase
{
    private readonly ILpDataService _service;
    public LpDataController(ILpDataService service)
    {
        _service = service;
    }

    [HttpGet("{id}")]
    public async Task<ApiResponse<LpData>> GetSingle([RequiredGuid] Guid id) => await _service.GetSingleAsync(id);
    [HttpPut("{id}")]
    public async Task<ApiResponse<LpData>> Update([RequiredGuid] Guid id, LpData dto) => await _service.UpdateAsync(id, dto);
}