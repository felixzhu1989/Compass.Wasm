using FluentValidation;

namespace Compass.Wasm.Server.IdentityService;

public record UpdateUserRequest(string UserName,string Email);
public class UpdateUserRequestValidator : AbstractValidator<UpdateUserRequest>
{
    public UpdateUserRequestValidator()
    {
        RuleFor(e => e.UserName).NotNull().NotEmpty().MaximumLength(20).MinimumLength(2);
        RuleFor(e => e.Email).NotNull().NotEmpty().MaximumLength(11);
    }
}