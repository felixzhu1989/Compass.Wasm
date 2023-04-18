using Compass.DataService.Infrastructure;
using Compass.Wasm.Shared;
using System.ComponentModel.DataAnnotations;
using Compass.Wasm.Shared.Data.Hoods;
using Compass.Wasm.Server.Services.Data.Hoods;

namespace Compass.Wasm.Server.Controllers.Data.Hoods;

[Route("api/[controller]")]
[ApiController]
[UnitOfWork(typeof(DataDbContext))]
public class UvfDataController : ControllerBase
{
    private readonly IUvfDataService _service;

    public UvfDataController(IUvfDataService service)
    {
        _service = service;
    }
    [HttpGet("{id}")]
    public async Task<ApiResponse<UvfData>> GetSingle([RequiredGuid] Guid id) => await _service.GetSingleAsync(id);
    [HttpPut("{id}")]
    public async Task<ApiResponse<UvfData>> Update([RequiredGuid] Guid id, UvfData dto) => await _service.UpdateAsync(id, dto);
}