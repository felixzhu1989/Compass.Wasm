using Compass.DataService.Infrastructure;
using Compass.Wasm.Shared.Data.Hoods;
using Compass.Wasm.Shared;
using System.ComponentModel.DataAnnotations;
using Compass.Wasm.Server.Services.Data.Hoods;

namespace Compass.Wasm.Server.Controllers.Data.Hoods;

[Route("api/[controller]")]
[ApiController]
[UnitOfWork(typeof(DataDbContext))]
public class CmodfDataController : ControllerBase
{
    private readonly ICmodfDataService _service;
    public CmodfDataController(ICmodfDataService service)
    {
        _service = service;
    }

    [HttpGet("{id}")]
    public async Task<ApiResponse<CmodfData>> GetSingle([RequiredGuid] Guid id) => await _service.GetSingleAsync(id);
    [HttpPut("{id}")]
    public async Task<ApiResponse<CmodfData>> Update([RequiredGuid] Guid id, CmodfData dto) => await _service.UpdateAsync(id, dto);
}