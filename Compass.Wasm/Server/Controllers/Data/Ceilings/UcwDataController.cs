using Compass.DataService.Infrastructure;
using Compass.Wasm.Server.Services.Data.Ceilings;
using Compass.Wasm.Shared.Data.Ceilings;
using System.ComponentModel.DataAnnotations;
using Compass.Wasm.Shared;

namespace Compass.Wasm.Server.Controllers.Data.Ceilings;

[Route("api/[controller]")]
[ApiController]
[UnitOfWork(typeof(DataDbContext))]
public class UcwDataController : ControllerBase
{
    private readonly IUcwDataService _service;
    public UcwDataController(IUcwDataService service)
    {
        _service = service;
    }
    [HttpGet("{id}")]
    public async Task<ApiResponse<UcwData>> GetSingle([RequiredGuid] Guid id) => await _service.GetSingleAsync(id);
    [HttpPut("{id}")]
    public async Task<ApiResponse<UcwData>> Update([RequiredGuid] Guid id, UcwData dto) => await _service.UpdateAsync(id, dto);
}