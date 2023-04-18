using System.Threading.Tasks;
using Compass.Wasm.Shared;
using Compass.Wasm.Shared.Identities;

namespace Compass.Wpf.ApiService;

public interface ILoginService
{
    Task<ApiResponse<UserDto>> LoginAsync(UserDto user);
}