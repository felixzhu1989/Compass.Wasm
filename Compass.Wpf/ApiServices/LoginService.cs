using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;
using Compass.Wasm.Shared;
using Compass.Wasm.Shared.Identities;
using Compass.Wpf.Common;

namespace Compass.Wpf.ApiService;

public class LoginService : ILoginService
{
    private readonly HttpRestClient _client;
    private readonly string serviceName = "Login";
    public LoginService(HttpRestClient client)
    {
        _client = client;
    }
    public async Task<ApiResponse<UserDto>> LoginAsync(UserDto user)
    {
        BaseRequest request = new()
        {
            Method=RestSharp.Method.Post,
            Route=$"api/{serviceName}/Name",
            Parameter=user
        };
        var loginResult = await _client.ExecuteAsync<string>(request);
        if (loginResult.Status)
        {
            //获取token，并从token中解析出用户名、id和角色
            var token = loginResult.Result;
            AppSession.Token=token;
            ClaimsIdentity identity = new(ParseClaimsFromJwt(token), "jwt");
            var id = identity.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value;
            var role = identity.Claims.First(x => x.Type == ClaimTypes.Role).Value;
            UserDto dto = new UserDto {Id = Guid.Parse(id),UserName = identity.Name,Roles = role};
            return new ApiResponse<UserDto> { Status = true, Result = dto };
        }
        return new ApiResponse<UserDto> { Status = false,Message = loginResult.Message};
    }


    #region 解析Token内容的代码
    public static IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
    {
        var payload = jwt.Split('.')[1];
        var jsonBytes = ParseBase64WithoutPadding(payload);
        var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);
        return keyValuePairs.Select(kvp => new Claim(kvp.Key, kvp.Value.ToString()));
    }
    private static byte[] ParseBase64WithoutPadding(string base64)
    {
        switch (base64.Length % 4)
        {
            case 2: base64 += "=="; break;
            case 3: base64 += "="; break;
        }
        return Convert.FromBase64String(base64);
    }
    #endregion
}