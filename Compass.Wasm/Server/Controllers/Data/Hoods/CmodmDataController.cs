using Compass.DataService.Infrastructure;
using Compass.Dtos;
using Compass.Wasm.Shared.Data.Hoods;
using System.ComponentModel.DataAnnotations;
using Compass.Wasm.Server.Services.Data.Hoods;

namespace Compass.Wasm.Server.Controllers.Data.Hoods;

[Route("api/[controller]")]
[ApiController]
[UnitOfWork(typeof(DataDbContext))]
public class CmodmDataController : ControllerBase
{
    private readonly ICmodmDataService _service;

    public CmodmDataController(ICmodmDataService service)
    {
        _service = service;
    }
    [HttpGet("{id}")]
    public async Task<ApiResponse<CmodmData>> GetSingle([RequiredGuid] Guid id) => await _service.GetSingleAsync(id);
    [HttpPut("{id}")]
    public async Task<ApiResponse<CmodmData>> Update([RequiredGuid] Guid id, CmodmData dto) => await _service.UpdateAsync(id, dto);
}