using Compass.DataService.Infrastructure;
using Compass.Dtos;
using Compass.Wasm.Server.Services.Data.Ceilings;
using Compass.Wasm.Shared.Data.Ceilings;
using System.ComponentModel.DataAnnotations;

namespace Compass.Wasm.Server.Controllers.Data.Ceilings;

[Route("api/[controller]")]
[ApiController]
[UnitOfWork(typeof(DataDbContext))]
public class SspDataController : ControllerBase
{
    private readonly ISspDataService _service;
    public SspDataController(ISspDataService service)
    {
        _service = service;
    }

    [HttpGet("{id}")]
    public async Task<ApiResponse<SspData>> GetSingle([RequiredGuid] Guid id) => await _service.GetSingleAsync(id);
    [HttpPut("{id}")]
    public async Task<ApiResponse<SspData>> Update([RequiredGuid] Guid id, SspData dto) => await _service.UpdateAsync(id, dto);

}