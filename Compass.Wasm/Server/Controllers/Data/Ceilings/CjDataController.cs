using Compass.DataService.Infrastructure;
using Compass.Wasm.Shared.Data.Ceilings;
using System.ComponentModel.DataAnnotations;
using Compass.Wasm.Server.Services.Data.Ceilings;
using Compass.Wasm.Shared;

namespace Compass.Wasm.Server.Controllers.Data.Ceilings;

[Route("api/[controller]")]
[ApiController]
[UnitOfWork(typeof(DataDbContext))]
public class CjDataController : ControllerBase
{
    private readonly ICjDataService _service;
    public CjDataController(ICjDataService service)
    {
        _service = service;
    }
    [HttpGet("{id}")]
    public async Task<ApiResponse<CjData>> GetSingle([RequiredGuid] Guid id) => await _service.GetSingleAsync(id);
    [HttpPut("{id}")]
    public async Task<ApiResponse<CjData>> Update([RequiredGuid] Guid id, CjData dto) => await _service.UpdateAsync(id, dto);
}