using FluentValidation;

namespace Compass.Wasm.Shared.IdentityService;

public record AddAdminRequest(string UserName, string Email);
public class AddAdminRequestValidator : AbstractValidator<AddAdminRequest>
{
    public AddAdminRequestValidator()
    {
        RuleFor(e => e.Email).NotNull().NotEmpty().MaximumLength(11);
        RuleFor(e => e.UserName).NotNull().NotEmpty().MaximumLength(20).MinimumLength(2);
    }
}