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
public class UwfDataController : ControllerBase
{
    private readonly IUwfDataService _service;
    public UwfDataController(IUwfDataService service)
    {
        _service = service;
    }

    [HttpGet("{id}")]
    public async Task<ApiResponse<UwfData>> GetSingle([RequiredGuid] Guid id) => await _service.GetSingleAsync(id);
    [HttpPut("{id}")]
    public async Task<ApiResponse<UwfData>> Update([RequiredGuid] Guid id, UwfData dto) => await _service.UpdateAsync(id, dto);
}