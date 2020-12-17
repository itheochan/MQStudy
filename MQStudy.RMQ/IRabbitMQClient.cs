using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace MQStudy.RMQ
{
    /// <summary>
    /// 消息队列客户端
    /// </summary>
    public interface IRabbitMQClient : IProducer, IConsumer
    {
        IConnection GetConnection();
    }
}
