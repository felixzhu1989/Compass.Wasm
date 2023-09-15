using Compass.DataService.Infrastructure;
using Compass.Wasm.Server.Services.Data.UL;
using Compass.Wasm.Shared.Data.UL;
using Compass.Wasm.Shared;
using System.ComponentModel.DataAnnotations;
using Compass.Dtos;

namespace Compass.Wasm.Server.Controllers.Data.UL;

[Route("api/[controller]")]
[ApiController]
[UnitOfWork(typeof(DataDbContext))]
public class KvcUvDataController : ControllerBase
{
    private readonly IKvcUvDataService _service;
    public KvcUvDataController(IKvcUvDataService service)
    {
        _service = service;
    }

    [HttpGet("{id}")]
    public async Task<ApiResponse<KvcUvData>> GetSingle([RequiredGuid] Guid id) => await _service.GetSingleAsync(id);
    [HttpPut("{id}")]
    public async Task<ApiResponse<KvcUvData>> Update([RequiredGuid] Guid id, KvcUvData dto) => await _service.UpdateAsync(id, dto);
}