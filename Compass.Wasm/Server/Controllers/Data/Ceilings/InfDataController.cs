using System.ComponentModel.DataAnnotations;
using Compass.DataService.Infrastructure;
using Compass.Dtos;
using Compass.Wasm.Server.Services.Data.Ceilings;
using Compass.Wasm.Shared.Data.Ceilings;

namespace Compass.Wasm.Server.Controllers.Data.Ceilings;

[Route("api/[controller]")]
[ApiController]
[UnitOfWork(typeof(DataDbContext))]
public class InfDataController : ControllerBase
{
    private readonly IInfDataService _service;
    public InfDataController(IInfDataService service)
    {
        _service = service;
    }

    [HttpGet("{id}")]
    public async Task<ApiResponse<InfData>> GetSingle([RequiredGuid] Guid id) => await _service.GetSingleAsync(id);
    [HttpPut("{id}")]
    public async Task<ApiResponse<InfData>> Update([RequiredGuid] Guid id, InfData dto) => await _service.UpdateAsync(id, dto);
}