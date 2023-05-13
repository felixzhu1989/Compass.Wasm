﻿using Compass.Wasm.Shared;
using Compass.Wasm.Shared.Plans;
using System.Net.Http.Json;

namespace Compass.Wasm.Client.Services.Plans;
public interface IIssueService : IBaseService<IssueDto>
{
    //扩展查询
    Task<ApiResponse<List<IssueDto>>> GetAllByMainPlanIdAsync(Guid mainPlanId);
}
public class IssueService : BaseService<IssueDto>, IIssueService
{
    private readonly HttpClient _http;
    public IssueService(HttpClient http) : base(http, "Issue")
    {
        _http = http;
    }

    public Task<ApiResponse<List<IssueDto>>> GetAllByMainPlanIdAsync(Guid mainPlanId)
    {
        return _http.GetFromJsonAsync<ApiResponse<List<IssueDto>>>($"api/Issue/MainPlan/{mainPlanId}")!;
    }
}