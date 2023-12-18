using Compass.DataService.Infrastructure;
using Compass.Wasm.Server.Services.Data.Ceilings;
using Compass.Wasm.Shared.Data.Ceilings;
using System.ComponentModel.DataAnnotations;
using Compass.Wasm.Shared;

namespace Compass.Wasm.Server.Controllers.Data.Ceilings;

[Route("api/[controller]")]
[ApiController]
[UnitOfWork(typeof(DataDbContext))]
public class LkDataController : ControllerBase
{
    private readonly ILkDataService _service;
    public LkDataController(ILkDataService service)
    {
        _service = service;
    }
    [HttpGet("{id}")]
    public async Task<ApiResponse<LkData>> GetSingle([RequiredGuid] Guid id) => await _service.GetSingleAsync(id);
    [HttpPut("{id}")]
    public async Task<ApiResponse<LkData>> Update([RequiredGuid] Guid id, LkData dto) => await _service.UpdateAsync(id, dto);
}