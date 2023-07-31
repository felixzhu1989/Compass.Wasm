using Compass.DataService.Infrastructure;
using Compass.Wasm.Shared;
using System.ComponentModel.DataAnnotations;
using Compass.Wasm.Server.Services.Data.UL;
using Compass.Wasm.Shared.Data.UL;

namespace Compass.Wasm.Server.Controllers.Data.UL;

[Route("api/[controller]")]
[ApiController]
[UnitOfWork(typeof(DataDbContext))]
public class ChDataController : ControllerBase
{
    private readonly IChDataService _service;
    public ChDataController(IChDataService service)
    {
        _service = service;
    }

    [HttpGet("{id}")]
    public async Task<ApiResponse<ChData>> GetSingle([RequiredGuid] Guid id) => await _service.GetSingleAsync(id);
    [HttpPut("{id}")]
    public async Task<ApiResponse<ChData>> Update([RequiredGuid] Guid id, ChData dto) => await _service.UpdateAsync(id, dto);
}