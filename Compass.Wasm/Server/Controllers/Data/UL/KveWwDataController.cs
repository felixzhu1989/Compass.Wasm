using Compass.DataService.Infrastructure;
using Compass.Wasm.Server.Services.Data.UL;
using Compass.Wasm.Shared.Data.UL;
using Compass.Wasm.Shared;
using System.ComponentModel.DataAnnotations;

namespace Compass.Wasm.Server.Controllers.Data.UL;

[Route("api/[controller]")]
[ApiController]
[UnitOfWork(typeof(DataDbContext))]
public class KveWwDataController : ControllerBase
{
    private readonly IKveWwDataService _service;
    public KveWwDataController(IKveWwDataService service)
    {
        _service = service;
    }

    [HttpGet("{id}")]
    public async Task<ApiResponse<KveWwData>> GetSingle([RequiredGuid] Guid id) => await _service.GetSingleAsync(id);
    [HttpPut("{id}")]
    public async Task<ApiResponse<KveWwData>> Update([RequiredGuid] Guid id, KveWwData dto) => await _service.UpdateAsync(id, dto);
}