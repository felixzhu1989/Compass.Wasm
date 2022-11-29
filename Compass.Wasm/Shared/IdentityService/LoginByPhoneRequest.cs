using FluentValidation;

namespace Compass.Wasm.Shared.IdentityService;

public record LoginByPhoneRequest(string PhoneNumber, string Password);
public class LoginByPhoneRequestValidator : AbstractValidator<LoginByPhoneRequest>
{
    public LoginByPhoneRequestValidator()
    {
        RuleFor(e => e.PhoneNumber).NotNull().NotEmpty();
        RuleFor(e => e.Password).NotNull().NotEmpty();
    }
}