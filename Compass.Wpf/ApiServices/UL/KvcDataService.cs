﻿using Compass.Wasm.Shared.Data.UL;
using Compass.Wpf.ApiService;
using Compass.Wpf.ApiServices.Projects;

namespace Compass.Wpf.ApiServices.UL;

public interface IKvcDataService : IBaseDataService<KvcData>
{
}

public class KvcDataService : BaseDataService<KvcData>, IKvcDataService
{
    public KvcDataService(HttpRestClient client, IModuleService moduleService) : base(client, "KvcData", moduleService)
    {
    }
}