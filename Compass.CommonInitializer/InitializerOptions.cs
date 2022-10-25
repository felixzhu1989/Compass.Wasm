namespace Compass.Wasm.CommonInitializer;

public class InitializerOptions
{
    //在Web.Api项目的program中初始化
    public string LogFilePath { get; set; }//程序日志存储地址
    //用于EventBus的QueueName，因此要维持“同一个项目值保持一直，不同项目不能冲突”
    public string EventBusQueueName { get; set; }//RabbitMQ队列名
}