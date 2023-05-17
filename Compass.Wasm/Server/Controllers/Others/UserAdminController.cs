using AutoMapper;
using Compass.Wasm.Shared.Identities;

namespace Compass.Wasm.Server.Controllers.Others;

[Route("api/[controller]")]
[ApiController]
//[Authorize(Roles = "admin")]
public class UserAdminController : ControllerBase
{
    private readonly IdentityUserManager _userManager;
    private readonly IEventBus _eventBus;
    private readonly IIdentityRepository _repository;
    private readonly IMapper _mapper;

    public UserAdminController(IdentityUserManager userManager, IIdentityRepository repository,IMapper mapper, IEventBus eventBus)
    {
        _userManager = userManager;
        _eventBus = eventBus;
        _repository = repository;
        _mapper = mapper;
    }

    [HttpGet("ByName/{userName}")]
    public async Task<ActionResult<UserDto>> FindByUserName(string userName)
    {
        var user = await _userManager.FindByNameAsync(userName);
        if (user == null) return NotFound();
        var roles = await _repository.GetRolesAsync(user);
        return new UserDto { Id=user.Id, UserName = user.UserName, Email= user.Email, Role=string.Join(',', roles), CreationTime = user.CreationTime };
    }
    [HttpGet("IdByName/{userName}")]
    public async Task<ActionResult<Guid>> GetIdByName(string userName)
    {
        var user = await _userManager.FindByNameAsync(userName);
        if (user == null) return NotFound();
        return user.Id;
    }
    [HttpGet("NameById/{id}")]
    public async Task<ActionResult<string>> GetNameById(Guid id)
    {
        var user = await _userManager.FindByIdAsync(id.ToString());
        if (user == null) return NotFound();
        return user.UserName;
    }
}