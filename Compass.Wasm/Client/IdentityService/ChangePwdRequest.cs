using FluentValidation;
namespace Compass.Wasm.Server.IdentityService;
public record ChangePwdRequest(string Password, string Password2);
public class ChangePwdRequestValidator : AbstractValidator<ChangePwdRequest>
{
    public ChangePwdRequestValidator()
    {
        RuleFor(e => e.Password).NotNull().NotEmpty()
            .Equal(e => e.Password2);//两次输入的密码必须相等
        RuleFor(e => e.Password2).NotNull().NotEmpty();
    }
}