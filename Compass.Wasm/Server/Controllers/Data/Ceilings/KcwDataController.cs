using Compass.DataService.Infrastructure;
using Compass.Dtos;
using Compass.Wasm.Shared.Data.Ceilings;
using System.ComponentModel.DataAnnotations;
using Compass.Wasm.Server.Services.Data.Ceilings;

namespace Compass.Wasm.Server.Controllers.Data.Ceilings;

[Route("api/[controller]")]
[ApiController]
[UnitOfWork(typeof(DataDbContext))]
public class KcwDataController : ControllerBase
{
    private readonly IKcwDataService _service;
    public KcwDataController(IKcwDataService service)
    {
        _service = service;
    }
    [HttpGet("{id}")]
    public async Task<ApiResponse<KcwData>> GetSingle([RequiredGuid] Guid id) => await _service.GetSingleAsync(id);
    [HttpPut("{id}")]
    public async Task<ApiResponse<KcwData>> Update([RequiredGuid] Guid id, KcwData dto) => await _service.UpdateAsync(id, dto);
}