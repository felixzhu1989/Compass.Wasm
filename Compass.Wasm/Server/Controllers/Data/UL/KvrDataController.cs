using Compass.DataService.Infrastructure;
using Compass.Wasm.Server.Services.Data.UL;
using Compass.Wasm.Shared.Data.UL;
using Compass.Wasm.Shared;
using System.ComponentModel.DataAnnotations;

namespace Compass.Wasm.Server.Controllers.Data.UL;

[Route("api/[controller]")]
[ApiController]
[UnitOfWork(typeof(DataDbContext))]
public class KvrDataController : ControllerBase
{
    private readonly IKvrDataService _service;
    public KvrDataController(IKvrDataService service)
    {
        _service = service;
    }

    [HttpGet("{id}")]
    public async Task<ApiResponse<KvrData>> GetSingle([RequiredGuid] Guid id) => await _service.GetSingleAsync(id);
    [HttpPut("{id}")]
    public async Task<ApiResponse<KvrData>> Update([RequiredGuid] Guid id, KvrData dto) => await _service.UpdateAsync(id, dto);
}