using Compass.Wasm.Shared;
using Compass.Wasm.Shared.IdentityService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Text.Json;
using AsmResolver.DotNet;
using Zack.DomainCommons.Models;


namespace Compass.Wpf.Service;

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
        //Todo:api返回的是token字符串，不是apiresponse，需要改造一番

        var loginResult = await _client.ExecuteAsync<string>(request);
        if (loginResult.Status)
        {
            //获取token
            var token = loginResult.Result;
            ClaimsIdentity identity = new(ParseClaimsFromJwt(token), "jwt");
            UserDto dto = new UserDto { UserName = identity.Name };
            return new ApiResponse<UserDto> { Status = true, Result = dto };
        }
        return new ApiResponse<UserDto> { Status = false,Message = "登录失败"};
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