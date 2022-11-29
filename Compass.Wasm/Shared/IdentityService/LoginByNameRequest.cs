using FluentValidation;

namespace Compass.Wasm.Shared.IdentityService;

public record LoginByNameRequest()
{
    public string UserName { get; set; }
    public string Password { get; set; }
}
//Install-Package FluentValidation.AspNetCore
public class LoginByNameRequestValidator : AbstractValidator<LoginByNameRequest>
{
    public LoginByNameRequestValidator()
    {
        RuleFor(e => e.UserName).NotNull().NotEmpty().WithMessage("用户名不能为空");
        RuleFor(e => e.Password).NotNull().NotEmpty().WithMessage("密码不能为空");
    }
}