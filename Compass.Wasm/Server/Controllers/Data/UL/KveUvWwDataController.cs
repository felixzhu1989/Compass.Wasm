using System.ComponentModel.DataAnnotations;
using Compass.DataService.Infrastructure;
using Compass.Wasm.Server.Services.Data.UL;
using Compass.Wasm.Shared;
using Compass.Wasm.Shared.Data.UL;

namespace Compass.Wasm.Server.Controllers.Data.UL;

[Route("api/[controller]")]
[ApiController]
[UnitOfWork(typeof(DataDbContext))]
public class KveUvWwDataController : ControllerBase
{
    private readonly IKveUvWwDataService _service;
    public KveUvWwDataController(IKveUvWwDataService service)
    {
        _service = service;
    }

    [HttpGet("{id}")]
    public async Task<ApiResponse<KveUvWwData>> GetSingle([RequiredGuid] Guid id) => await _service.GetSingleAsync(id);
    [HttpPut("{id}")]
    public async Task<ApiResponse<KveUvWwData>> Update([RequiredGuid] Guid id, KveUvWwData dto) => await _service.UpdateAsync(id, dto);
}