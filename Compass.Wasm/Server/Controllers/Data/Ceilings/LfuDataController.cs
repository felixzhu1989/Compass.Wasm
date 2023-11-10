using Compass.DataService.Infrastructure;
using Compass.Dtos;
using Compass.Wasm.Server.Services.Data.Ceilings;
using Compass.Wasm.Shared.Data.Ceilings;
using System.ComponentModel.DataAnnotations;

namespace Compass.Wasm.Server.Controllers.Data.Ceilings;

[Route("api/[controller]")]
[ApiController]
[UnitOfWork(typeof(DataDbContext))]
public class LfuDataController : ControllerBase
{
    private readonly ILfuDataService _service;
    public LfuDataController(ILfuDataService service)
    {
        _service = service;
    }

    [HttpGet("{id}")]
    public async Task<ApiResponse<LfuData>> GetSingle([RequiredGuid] Guid id) => await _service.GetSingleAsync(id);
    [HttpPut("{id}")]
    public async Task<ApiResponse<LfuData>> Update([RequiredGuid] Guid id, LfuData dto) => await _service.UpdateAsync(id, dto);

}