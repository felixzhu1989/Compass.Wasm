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
public class KvcWwDataController : ControllerBase
{
    private readonly IKvcWwDataService _service;
    public KvcWwDataController(IKvcWwDataService service)
    {
        _service = service;
    }

    [HttpGet("{id}")]
    public async Task<ApiResponse<KvcWwData>> GetSingle([RequiredGuid] Guid id) => await _service.GetSingleAsync(id);
    [HttpPut("{id}")]
    public async Task<ApiResponse<KvcWwData>> Update([RequiredGuid] Guid id, KvcWwData dto) => await _service.UpdateAsync(id, dto);
}