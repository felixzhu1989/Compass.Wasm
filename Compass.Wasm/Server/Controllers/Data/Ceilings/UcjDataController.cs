using Compass.DataService.Infrastructure;
using Compass.Wasm.Server.Services.Data.Ceilings;
using Compass.Wasm.Shared.Data.Ceilings;
using System.ComponentModel.DataAnnotations;
using Compass.Wasm.Shared;

namespace Compass.Wasm.Server.Controllers.Data.Ceilings;

[Route("api/[controller]")]
[ApiController]
[UnitOfWork(typeof(DataDbContext))]
public class UcjDataController : ControllerBase
{
    private readonly IUcjDataService _service;
    public UcjDataController(IUcjDataService service)
    {
        _service = service;
    }

    [HttpGet("{id}")]
    public async Task<ApiResponse<UcjData>> GetSingle([RequiredGuid] Guid id) => await _service.GetSingleAsync(id);
    [HttpPut("{id}")]
    public async Task<ApiResponse<UcjData>> Update([RequiredGuid] Guid id, UcjData dto) => await _service.UpdateAsync(id, dto);
}