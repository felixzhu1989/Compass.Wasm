using Compass.DataService.Infrastructure;
using Compass.Wasm.Shared;
using System.ComponentModel.DataAnnotations;
using Compass.Wasm.Shared.Data.Hoods;
using Compass.Wasm.Server.Services.Data.Hoods;

namespace Compass.Wasm.Server.Controllers.Data.Hoods;

[Route("api/[controller]")]
[ApiController]
[UnitOfWork(typeof(DataDbContext))]
public class KvfDataController : ControllerBase
{
    private readonly IKvfDataService _service;

    public KvfDataController(IKvfDataService service)
    {
        _service = service;
    }
    [HttpGet("{id}")]
    public async Task<ApiResponse<KvfData>> GetSingle([RequiredGuid] Guid id) => await _service.GetSingleAsync(id);
    [HttpPut("{id}")]
    public async Task<ApiResponse<KvfData>> Update([RequiredGuid] Guid id, KvfData dto) => await _service.UpdateAsync(id, dto);
}