using Compass.DataService.Infrastructure;
using Compass.Wasm.Shared.DataService.Hoods;
using Compass.Wasm.Shared;
using System.ComponentModel.DataAnnotations;
using Compass.Wasm.Server.HoodService;

namespace Compass.Wasm.Server.Controllers.HoodService;

[Route("api/[controller]")]
[ApiController]
[UnitOfWork(typeof(DataDbContext))]
public class UviDataController : ControllerBase
{
    private readonly IUviDataService _service;
    public UviDataController(IUviDataService service)
    {
        _service = service;
    }

    [HttpGet("{id}")]
    public async Task<ApiResponse<UviData>> GetSingle([RequiredGuid] Guid id) => await _service.GetSingleAsync(id);
    [HttpPut("{id}")]
    public async Task<ApiResponse<UviData>> Update([RequiredGuid] Guid id, UviData dto) => await _service.UpdateAsync(id, dto);

}