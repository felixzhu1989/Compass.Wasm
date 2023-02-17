﻿using Compass.Wasm.Shared;
using RestSharp;
using System.Threading.Tasks;
using System;
using Newtonsoft.Json;
using System.Net;
using Newtonsoft.Json.Linq;
using Compass.Wpf.Common;

namespace Compass.Wpf.Service;

/// <summary>
/// 封装RestSharp请求代码
/// </summary>
public class HttpRestClient
{
    private readonly string _apiUrl;
    protected readonly RestClient _client;
    public HttpRestClient(string apiUrl)
    {
        _apiUrl=apiUrl;
        _client=new RestClient();
    }

    public async Task<ApiResponse<T>> ExecuteAsync<T>(BaseRequest baseRequest)
    {
        
        var resource = new Uri($"{_apiUrl}{baseRequest.Route}");
        var request = new RestRequest(resource, baseRequest.Method);
        request.AddHeader("Content-Type", baseRequest.ContentType);
        //获取token，给请求添加token
        var token = AppSession.Token;
        if (!string.IsNullOrEmpty(token))
        {
            request.AddOrUpdateHeader("Authorization", $"Bearer {token.Replace("\"", "")}");
        }
        //传递的参数，必须使用Newtonsoft.Json，不能使用微软自带Json
        if (baseRequest.Parameter != null) request.AddJsonBody(baseRequest.Parameter);
        var response = await _client.ExecuteAsync(request);
        if (response.StatusCode == HttpStatusCode.OK)
        {
            return JsonConvert.DeserializeObject<ApiResponse<T>>(response.Content!)!;
        }
        return new ApiResponse<T>{Status = false,Message = response.ErrorMessage};
    }
}