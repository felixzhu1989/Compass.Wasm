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
public class KveUvDataController : ControllerBase
{
    private readonly IKveUvDataService _service;
    public KveUvDataController(IKveUvDataService service)
    {
        _service = service;
    }

    [HttpGet("{id}")]
    public async Task<ApiResponse<KveUvData>> GetSingle([RequiredGuid] Guid id) => await _service.GetSingleAsync(id);
    [HttpPut("{id}")]
    public async Task<ApiResponse<KveUvData>> Update([RequiredGuid] Guid id, KveUvData dto) => await _service.UpdateAsync(id, dto);
}