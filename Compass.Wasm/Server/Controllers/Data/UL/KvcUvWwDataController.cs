using System.ComponentModel.DataAnnotations;
using Compass.DataService.Infrastructure;
using Compass.Wasm.Server.Services.Data.UL;
using Compass.Wasm.Shared;
using Compass.Wasm.Shared.Data.UL;

namespace Compass.Wasm.Server.Controllers.Data.UL;

[Route("api/[controller]")]
[ApiController]
[UnitOfWork(typeof(DataDbContext))]
public class KvcUvWwDataController : ControllerBase
{
    private readonly IKvcUvWwDataService _service;
    public KvcUvWwDataController(IKvcUvWwDataService service)
    {
        _service = service;
    }

    [HttpGet("{id}")]
    public async Task<ApiResponse<KvcUvWwData>> GetSingle([RequiredGuid] Guid id) => await _service.GetSingleAsync(id);
    [HttpPut("{id}")]
    public async Task<ApiResponse<KvcUvWwData>> Update([RequiredGuid] Guid id, KvcUvWwData dto) => await _service.UpdateAsync(id, dto);
}