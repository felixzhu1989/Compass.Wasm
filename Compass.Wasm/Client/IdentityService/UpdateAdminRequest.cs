﻿using FluentValidation;

namespace Compass.Wasm.Server.IdentityService;

public record UpdateAdminRequest(string UserName,string Email);
public class UpdateAdminRequestValidator : AbstractValidator<UpdateAdminRequest>
{
    public UpdateAdminRequestValidator()
    {
        RuleFor(e => e.UserName).NotNull().NotEmpty().MaximumLength(20).MinimumLength(2);
        RuleFor(e => e.Email).NotNull().NotEmpty().MaximumLength(11);
    }
}