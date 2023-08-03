﻿using Compass.Wasm.Shared.Data.Hoods;
using Compass.Wpf.ApiService;
using Compass.Wpf.ApiServices.Projects;

namespace Compass.Wpf.ApiServices.Hoods;

public interface IKchDataService : IBaseDataService<KchData>
{
}

public class KchDataService : BaseDataService<KchData>, IKchDataService
{
    public KchDataService(HttpRestClient client, IModuleService moduleService) : base(client, "KchData", moduleService)
    {
    }
}