using Compass.Wasm.Shared.IdentityService;
using Compass.Wasm.Shared;
using System.Threading.Tasks;

namespace Compass.Wpf.Service;

public interface ILoginService
{
    Task<ApiResponse<UserDto>> LoginAsync(UserDto user);
}