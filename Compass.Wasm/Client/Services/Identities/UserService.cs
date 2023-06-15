using Compass.Wasm.Shared;
using Compass.Wasm.Shared.Identities;
using System.Net.Http.Json;

namespace Compass.Wasm.Client.Services.Identities;
public interface IUserService : IBaseService<UserDto>
{
    Task<ApiResponse<List<UserDto>>> GetUsersInRolesAsync(string roles);
    Task<HttpResponseMessage> ResetPwdAsync(Guid id, UserDto dto);
    Task<HttpResponseMessage> ChangePwdAsync(Guid id, UserDto dto);
}
public class UserService : BaseService<UserDto>, IUserService
{
    private readonly HttpClient _http;
    public UserService(HttpClient http) : base(http, "User")
    {
        _http = http;
    }
    public Task<ApiResponse<List<UserDto>>> GetUsersInRolesAsync(string roles)
    {
        return _http.GetFromJsonAsync<ApiResponse<List<UserDto>>>($"api/User/Roles/{roles}")!;
    }

    public Task<HttpResponseMessage> ResetPwdAsync(Guid id, UserDto dto)
    {
        return _http.PutAsJsonAsync($"api/User/ResetPwd/{id}", dto);
    }

    public Task<HttpResponseMessage> ChangePwdAsync(Guid id, UserDto dto)
    {
        return _http.PutAsJsonAsync($"api/User/ChangePwd/{id}", dto);
    }
}