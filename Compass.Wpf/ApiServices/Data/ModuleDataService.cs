﻿using Compass.Wasm.Shared.Data;

namespace Compass.Wpf.ApiService;

public class ModuleDataService:BaseService<ModuleData>,IModuleDataService
{
    public ModuleDataService(HttpRestClient client) : base(client, "ModuleData")
    {
    }
}