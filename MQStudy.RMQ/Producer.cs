using RabbitMQ.Client;
using RabbitMQ.Client.Core.DependencyInjection.Services;
using System.Text;
using System.Threading.Tasks;
public class Producer
{
    private readonly IQueueService _queueService;

    public Producer(IQueueService queueService)
    {
        _queueService = queueService;
    }

    public async Task SendAsync()
    {
        var messageObject = new { Id = 1, Body = "Hello DependencyInjection!" };
        await _queueService.SendAsync(@object: messageObject, exchangeName: "", routingKey: "hello");
    }
    public void Publish()
    {
        var factory = new ConnectionFactory() { HostName = "localhost", UserName = "admin", Password = "admin", VirtualHost = "/" };
        using var connection = factory.CreateConnection();
        using var channel = connection.CreateModel();
        channel.QueueDeclare(queue: "hello", durable: false, exclusive: false, autoDelete: false);
        string message = "Hello World!";
        var body = Encoding.UTF8.GetBytes(message);
        channel.BasicPublish(exchange: "", routingKey: "hello", basicProperties: null, body: body);
    }
}
