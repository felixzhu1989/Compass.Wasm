using Compass.DataService.Infrastructure;
using Compass.Wasm.Server.Services.Data.Hoods;
using Compass.Wasm.Shared.Data.UL;
using Compass.Wasm.Shared;
using System.ComponentModel.DataAnnotations;
using Compass.Dtos;
using Compass.Wasm.Shared.Data.Hoods;

namespace Compass.Wasm.Server.Controllers.Data.Hoods;

[Route("api/[controller]")]
[ApiController]
[UnitOfWork(typeof(DataDbContext))]
public class UvimDataController : ControllerBase
{
    private readonly IUvimDataService _service;
    public UvimDataController(IUvimDataService service)
    {
        _service = service;
    }
    [HttpGet("{id}")]
    public async Task<ApiResponse<UvimData>> GetSingle([RequiredGuid] Guid id) => await _service.GetSingleAsync(id);
    [HttpPut("{id}")]
    public async Task<ApiResponse<UvimData>> Update([RequiredGuid] Guid id, UvimData dto) => await _service.UpdateAsync(id, dto);
}