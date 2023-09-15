using Compass.DataService.Infrastructure;
using Compass.Wasm.Server.Services.Data.Hoods;
using Compass.Wasm.Shared.Data.Hoods;
using Compass.Wasm.Shared;
using System.ComponentModel.DataAnnotations;
using Compass.Dtos;

namespace Compass.Wasm.Server.Controllers.Data.Hoods;

[Route("api/[controller]")]
[ApiController]
[UnitOfWork(typeof(DataDbContext))]
public class KwfDataController : ControllerBase
{
    private readonly IKwfDataService _service;
    public KwfDataController(IKwfDataService service)
    {
        _service = service;
    }

    [HttpGet("{id}")]
    public async Task<ApiResponse<KwfData>> GetSingle([RequiredGuid] Guid id) => await _service.GetSingleAsync(id);
    [HttpPut("{id}")]
    public async Task<ApiResponse<KwfData>> Update([RequiredGuid] Guid id, KwfData dto) => await _service.UpdateAsync(id, dto);
}
