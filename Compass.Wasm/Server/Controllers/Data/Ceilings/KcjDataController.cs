using Compass.DataService.Infrastructure;
using Compass.Wasm.Server.Services.Data.Ceilings;
using Compass.Wasm.Shared;
using System.ComponentModel.DataAnnotations;
using Compass.Wasm.Shared.Data.Ceilings;

namespace Compass.Wasm.Server.Controllers.Data.Ceilings;

[Route("api/[controller]")]
[ApiController]
[UnitOfWork(typeof(DataDbContext))]
public class KcjDataController : ControllerBase
{
    private readonly IKcjDataService _service;
    public KcjDataController(IKcjDataService service)
    {
        _service = service;
    }
    [HttpGet("{id}")]
    public async Task<ApiResponse<KcjData>> GetSingle([RequiredGuid] Guid id) => await _service.GetSingleAsync(id);
    [HttpPut("{id}")]
    public async Task<ApiResponse<KcjData>> Update([RequiredGuid] Guid id, KcjData dto) => await _service.UpdateAsync(id, dto);
}