using Compass.DataService.Infrastructure;
using Compass.Dtos;
using Compass.Wasm.Server.Services.Data.Ceilings;
using Compass.Wasm.Shared.Data.Ceilings;
using System.ComponentModel.DataAnnotations;

namespace Compass.Wasm.Server.Controllers.Data.Ceilings;

[Route("api/[controller]")]
[ApiController]
[UnitOfWork(typeof(DataDbContext))]
public class DpDataController : ControllerBase
{
    private readonly IDpDataService _service;
    public DpDataController(IDpDataService service)
    {
        _service = service;
    }
    [HttpGet("{id}")]
    public async Task<ApiResponse<DpData>> GetSingle([RequiredGuid] Guid id) => await _service.GetSingleAsync(id);
    [HttpPut("{id}")]
    public async Task<ApiResponse<DpData>> Update([RequiredGuid] Guid id, DpData dto) => await _service.UpdateAsync(id, dto);
}