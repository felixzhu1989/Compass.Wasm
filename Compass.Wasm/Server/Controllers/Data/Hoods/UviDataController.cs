using Compass.DataService.Infrastructure;
using Compass.Wasm.Shared;
using System.ComponentModel.DataAnnotations;
using Compass.Dtos;
using Compass.Wasm.Shared.Data.Hoods;
using Compass.Wasm.Server.Services.Data.Hoods;

namespace Compass.Wasm.Server.Controllers.Data.Hoods;

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