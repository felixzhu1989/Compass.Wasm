using System.ComponentModel.DataAnnotations;
using Compass.DataService.Infrastructure;
using Compass.Dtos;
using Compass.Wasm.Server.Services.Data.UL;
using Compass.Wasm.Shared;
using Compass.Wasm.Shared.Data.UL;

namespace Compass.Wasm.Server.Controllers.Data.UL;

[Route("api/[controller]")]
[ApiController]
[UnitOfWork(typeof(DataDbContext))]
public class KveDataController : ControllerBase
{
    private readonly IKveDataService _service;
    public KveDataController(IKveDataService service)
    {
        _service = service;
    }

    [HttpGet("{id}")]
    public async Task<ApiResponse<KveData>> GetSingle([RequiredGuid] Guid id) => await _service.GetSingleAsync(id);
    [HttpPut("{id}")]
    public async Task<ApiResponse<KveData>> Update([RequiredGuid] Guid id, KveData dto) => await _service.UpdateAsync(id, dto);
}