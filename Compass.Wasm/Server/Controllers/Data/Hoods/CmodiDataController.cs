using Compass.DataService.Infrastructure;
using Compass.Wasm.Server.Services.Data.Hoods;
using Compass.Wasm.Shared.Data.Hoods;
using Compass.Wasm.Shared;
using System.ComponentModel.DataAnnotations;
using Compass.Dtos;

namespace Compass.Wasm.Server.Controllers.Data.Hoods;

[Route("api/[controller]")]
[ApiController]
[UnitOfWork(typeof(DataDbContext))]
public class CmodiDataController : ControllerBase
{
    private readonly ICmodiDataService _service;
    public CmodiDataController(ICmodiDataService service)
    {
        _service = service;
    }

    [HttpGet("{id}")]
    public async Task<ApiResponse<CmodiData>> GetSingle([RequiredGuid] Guid id) => await _service.GetSingleAsync(id);
    [HttpPut("{id}")]
    public async Task<ApiResponse<CmodiData>> Update([RequiredGuid] Guid id, CmodiData dto) => await _service.UpdateAsync(id, dto);
}