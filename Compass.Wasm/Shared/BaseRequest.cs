﻿using RestSharp;

namespace Compass.Wasm.Shared;

public class BaseRequest
{
    public Method Method { get; set; }
    public string Route { get; set; }
    public string ContentType { get; set; } = "application/json";
    public object Param { get; set; }
}