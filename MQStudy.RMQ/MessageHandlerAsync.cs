/*
* --------------------------------------------------
* Copyright(C)		: 
* Namespace			: MQStudy.RMQ
* FileName			: MessageHandlerAsync.cs
* CLR Version		: 4.0.30319.42000
* Author			: iTheo
* CreateTime		: 2020/12/6 22:50:54
* --------------------------------------------------
*/
using Microsoft.Extensions.Logging;
using RabbitMQ.Client.Core.DependencyInjection.MessageHandlers;
using RabbitMQ.Client.Events;
using System.Text;
using System.Threading.Tasks;

public class MessageHandlerAsync : IAsyncMessageHandler//IMessageHandler
{
    private readonly ILogger<MessageHandlerAsync> _logger;

    public MessageHandlerAsync(ILogger<MessageHandlerAsync> logger)
    {
        _logger = logger;
    }

    public async Task Handle(BasicDeliverEventArgs eventArgs, string matchingRoute)
    {
        var message = Encoding.UTF8.GetString(eventArgs.Body.Span);
        _logger.LogInformation($"RoutingKey: {eventArgs.RoutingKey}, Message:{message}");
    }
}
