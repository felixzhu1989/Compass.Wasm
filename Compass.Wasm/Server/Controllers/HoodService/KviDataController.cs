using Compass.DataService.Infrastructure;
using Compass.Wasm.Server.HoodService;
using Compass.Wasm.Shared;
using Compass.Wasm.Shared.DataService.Hoods;
using System.ComponentModel.DataAnnotations;

namespace Compass.Wasm.Server.Controllers.HoodService
{
    [Route("api/[controller]")]
    [ApiController]
    [UnitOfWork(typeof(DataDbContext))]
    public class KviDataController : ControllerBase
    {
        private readonly IKviDataService _service;
        public KviDataController(IKviDataService service)
        {
            _service = service;
        }

        [HttpGet("{id}")]
        public async Task<ApiResponse<KviData>> GetSingle([RequiredGuid] Guid id) => await _service.GetSingleAsync(id);
        [HttpPut("{id}")]
        public async Task<ApiResponse<KviData>> Update([RequiredGuid] Guid id, KviData dto) => await _service.UpdateAsync(id, dto);
    }
}
