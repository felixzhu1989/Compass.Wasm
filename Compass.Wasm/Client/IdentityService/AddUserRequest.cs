using FluentValidation;

namespace Compass.Wasm.Server.IdentityService;

public record AddUserRequest(string UserName, string Email, string RoleName);
public class AddUserRequestValidator : AbstractValidator<AddUserRequest>
{
    public AddUserRequestValidator()
    {
        RuleFor(e => e.Email).NotNull().NotEmpty().MaximumLength(11);
        RuleFor(e => e.UserName).NotNull().NotEmpty().MaximumLength(20).MinimumLength(2);
        RuleFor(e => e.RoleName).NotNull().NotEmpty();
    }
}