﻿namespace Compass.Wasm.Shared.IdentityService;
public record UserResponse(Guid Id, string UserName, string Email, string Roles,DateTime CreationTime);