using Compass.DataService.Infrastructure;
using Compass.Wasm.Server.Services.Data.UL;
using Compass.Wasm.Shared.Data.UL;
using Compass.Wasm.Shared;
using System.ComponentModel.DataAnnotations;
using Compass.Dtos;

namespace Compass.Wasm.Server.Controllers.Data.UL;

[Route("api/[controller]")]
[ApiController]
[UnitOfWork(typeof(DataDbContext))]
public class KvwController : ControllerBase
{
    private readonly IKvwDataService _service;
    public KvwController(IKvwDataService service)
    {
        _service = service;
    }
    [HttpGet("{id}")]
    public async Task<ApiResponse<KvwData>> GetSingle([RequiredGuid] Guid id) => await _service.GetSingleAsync(id);
    [HttpPut("{id}")]
    public async Task<ApiResponse<KvwData>> Update([RequiredGuid] Guid id, KvwData dto) => await _service.UpdateAsync(id, dto);
}