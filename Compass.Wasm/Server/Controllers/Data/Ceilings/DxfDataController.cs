using System.ComponentModel.DataAnnotations;
using Compass.DataService.Infrastructure;
using Compass.Dtos;
using Compass.Wasm.Server.Services.Data.Ceilings;
using Compass.Wasm.Shared.Data.Ceilings;

namespace Compass.Wasm.Server.Controllers.Data.Ceilings;

[Route("api/[controller]")]
[ApiController]
[UnitOfWork(typeof(DataDbContext))]
public class DxfDataController : ControllerBase
{
    private readonly IDxfDataService _service;
    public DxfDataController(IDxfDataService service)
    {
        _service = service;
    }

    [HttpGet("{id}")]
    public async Task<ApiResponse<DxfData>> GetSingle([RequiredGuid] Guid id) => await _service.GetSingleAsync(id);
    [HttpPut("{id}")]
    public async Task<ApiResponse<DxfData>> Update([RequiredGuid] Guid id, DxfData dto) => await _service.UpdateAsync(id, dto);
}