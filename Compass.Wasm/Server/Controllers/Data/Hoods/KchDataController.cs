using Compass.DataService.Infrastructure;
using Compass.Wasm.Shared;
using System.ComponentModel.DataAnnotations;
using Compass.Wasm.Shared.Data.Hoods;
using Compass.Wasm.Server.Services.Data.Hoods;

namespace Compass.Wasm.Server.Controllers.Data.Hoods;

[Route("api/[controller]")]
[ApiController]
[UnitOfWork(typeof(DataDbContext))]
public class KchDataController : ControllerBase
{
    private readonly IKchDataService _service;

    public KchDataController(IKchDataService service)
    {
        _service = service;
    }
    [HttpGet("{id}")]
    public async Task<ApiResponse<KchData>> GetSingle([RequiredGuid] Guid id) => await _service.GetSingleAsync(id);
    [HttpPut("{id}")]
    public async Task<ApiResponse<KchData>> Update([RequiredGuid] Guid id, KchData dto) => await _service.UpdateAsync(id, dto);
}