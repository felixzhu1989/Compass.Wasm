using Microsoft.AspNetCore.Builder;
using Zack.EventBus;

namespace Compass.Wasm.CommonInitializer;
public static class ApplicationBuilderExtensions
{
    //IApplicationBuilder扩展方法，添加各个微服务(WebApi)都需要添加的中间件
    public static IApplicationBuilder UseCompassDefault(this IApplicationBuilder app)
    {
        app.UseDeveloperExceptionPage();
        app.UseSwagger();
        app.UseSwaggerUI();

        //NuGet安装：Install-Package Zack.EventBus
        app.UseEventBus();//简化集成事件的框架
        //app.UseCors();//启用Cors，跨域访问
        //app.UseForwardedHeaders();//Nginx？暂时不会配置Blazor相关的Nginx设置，放弃
        app.UseAuthentication();//JWT
        app.UseAuthorization();
        return app;
    }
}