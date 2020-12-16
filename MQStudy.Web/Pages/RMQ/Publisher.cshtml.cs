using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Core.DependencyInjection.Services;
using System;
using System.Text;

namespace MQStudy.Web.Pages.RMQ
{
    public class PublisherModel : PageModel
    {
        private readonly ILogger<PublisherModel> _logger;
        private readonly IQueueService _queueService;
        public PublisherModel(ILogger<PublisherModel> logger, IQueueService queueService)
        {
            _logger = logger;
            _queueService = queueService;
        }

        public void OnGet()
        {
        }

        public void Publish()
        {
            var factory = new ConnectionFactory() { HostName = "localhost", UserName="admin", Password="admin", VirtualHost="/" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: "hello", durable: false, exclusive: false, autoDelete: false);
                string message = "Hello World!";
                var body = Encoding.UTF8.GetBytes(message);
                channel.BasicPublish(exchange: "", routingKey: "hello", basicProperties: null, body: body);
                Console.WriteLine(" [x] Sent {0}", message);
            }
        }
    }
}
