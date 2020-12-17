/*
* --------------------------------------------------
* Copyright(C)		: 
* Namespace			: MQStudy.RMQ
* FileName			: RabbitMQClient.cs
* CLR Version		: 4.0.30319.42000
* Author			: Theo
* CreateTime		: 2020/12/17 10:50:19
* --------------------------------------------------
*/
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MQStudy.RMQ
{
    ///<summary>
    /// RabbitMQClient
    ///</summary>
    public class RabbitMQClient : IRabbitMQClient
    {
        #region Fields 
        private readonly ILogger<RabbitMQClient> _logger;
        #endregion Fields

        #region Ctor

        public RabbitMQClient(ILogger<RabbitMQClient> logger)
        {
            _logger = logger;
        }
        #endregion Ctor

        #region Properties
        private static RabbitMQOptions Options { get; set; }
        public static ConnectionFactory ConnectionFactory { get; private set; }
        public static IList<string> HostNames { get; private set; }
        public static IList<AmqpTcpEndpoint> EndPoints { get; private set; }
        #endregion Properties

        #region Methods
        internal static void Init(RabbitMQOptions options)
        {
            Options = options ?? throw new ArgumentNullException(nameof(options));
            ConnectionFactory = CreateFactory(options);
            if (options.HostNames?.Count > 0)
            {
                HostNames = options.HostNames;
            }

            if (options.Endpoints?.Count > 0)
            {
                EndPoints = (from dic in options.Endpoints
                             select new AmqpTcpEndpoint()
                             {
                                 HostName = dic.Key,
                                 Port = dic.Value
                             }).ToList();
            }
        }

        private static ConnectionFactory CreateFactory(RabbitMQOptions options)
        {
            ConnectionFactory factory = new ConnectionFactory()
            {
                HostName = options.HostName,
                Port = options.Port,
                UserName = options.UserName,
                Password = options.Password,
                VirtualHost = options.VirtualHost,
                //  ClientProvidedName = options.ClientProvidedName
            };

            if (!string.IsNullOrWhiteSpace(options.ClientProvidedName))
            { factory.ClientProvidedName = options.ClientProvidedName; }
            if (options.ConsumerDispatchConcurrency > 0)
            { factory.ConsumerDispatchConcurrency = options.ConsumerDispatchConcurrency; }
            return factory;
        }

        /// <summary>
        /// 获取连接
        /// </summary>
        /// <returns></returns>
        public IConnection GetConnection()
        {
            return GetConnection(ConnectionFactory);
        }

        /// <summary>
        /// 获取连接
        /// </summary>
        /// <returns></returns>
        private IConnection GetConnection(ConnectionFactory factory)
        {
            if (string.IsNullOrWhiteSpace(factory.ClientProvidedName))
            {
                return EndPoints == null ?
                    (HostNames == null) ?
                    factory.CreateConnection() :
                    factory.CreateConnection(HostNames) :
                    factory.CreateConnection(EndPoints);
            }
            else
            {
                return EndPoints == null ?
                    (HostNames == null) ?
                    factory.CreateConnection(factory.ClientProvidedName) :
                    factory.CreateConnection(HostNames, factory.ClientProvidedName) :
                    factory.CreateConnection(EndPoints, factory.ClientProvidedName);
            }
        }

        /// <summary>
        /// 发布消息
        /// </summary>
        /// <param name="message">消息内容</param>
        /// <param name="exchange">交换机</param>
        /// <param name="queue">队列</param>
        /// <param name="routingKey">路由</param>
        public void Publish(string message, string queue, string exchange = "", string routingKey = null)
        {
            if (string.IsNullOrWhiteSpace(queue))
            {
                throw new ArgumentNullException(nameof(queue));
            }
            using var connection = GetConnection();
            using var channel = connection.CreateModel();
            if (!string.IsNullOrWhiteSpace(queue))
            {
                channel.QueueDeclare(queue: queue,
                                     durable: true,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);
            }
            var bytes = Encoding.UTF8.GetBytes(message);
            channel.BasicPublish(exchange: exchange ?? "", routingKey: routingKey, basicProperties: null, body: bytes);
        }

        /// <summary>
        /// 异步发布消息
        /// </summary>
        /// <param name="message">消息内容</param>
        /// <param name="exchange">交换机</param>
        /// <param name="queue">队列</param>
        /// <param name="routingKey">路由</param>
        /// <returns></returns>
        public Task PublishAsync(string message, string queue, string exchange = "", string routingKey = null)
        {
            if (string.IsNullOrWhiteSpace(queue))
            {
                throw new ArgumentNullException(nameof(queue));
            }
            TaskCompletionSource tcs = new TaskCompletionSource();
            ThreadPool.QueueUserWorkItem(_ =>
            {
                try
                {
                    Publish(message, queue, exchange, routingKey);
                    tcs.SetResult();
                }
                catch (Exception ex)
                {
                    tcs.SetException(ex);
                }
            });
            return tcs.Task;
        }

        /// <summary>
        /// Consume
        /// </summary>
        /// <param name="queue">队列</param>
        /// <param name="handler">消费委托</param>
        /// <param name="holdThread">是否保持线程, 默认true</param>
        public void Consume(string queue, Action<string> handler, bool holdThread = true)
        {
            using var connection = GetConnection();
            using var channel = connection.CreateModel();
            channel.QueueDeclare(queue: queue,
                                 durable: true,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);
            //输入1，那如果接收一个消息，但是没有应答，则客户端不会收到下一个消息
            channel.BasicQos(0, 1, false);
            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                handler(message);
                //手动应答
                channel.BasicAck(ea.DeliveryTag, false);
            };
            //开始消费
            string actualConsumerTag = channel.BasicConsume(queue, false, consumer);
            _logger.LogInformation($"queue[{queue}] start consume. ActualConsumerTag: {actualConsumerTag}");
            //保持线程
            while (holdThread)
            {
                Thread.Sleep(10000);
            }
        }

        /// <summary>
        /// Consume
        /// </summary>
        /// <param name="queue">队列</param>
        /// <param name="handler">消费委托</param>
        /// <param name="consumerDispatchConcurrency">消费者并发数</param>
        /// <param name="holdThread">是否保持线程, 默认true</param>
        public void Consume(string queue, Action<string> handler, int consumerDispatchConcurrency, bool holdThread = true)
        {
            var factory = CreateFactory(Options);
            factory.ConsumerDispatchConcurrency = consumerDispatchConcurrency;
            using var connection = GetConnection(factory);
            using var channel = connection.CreateModel();
            channel.QueueDeclare(queue: queue,
                                 durable: true,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);
            //输入1，那如果接收一个消息，但是没有应答，则客户端不会收到下一个消息
            channel.BasicQos(0, 1, false);
            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                handler(message);
                //手动应答
                channel.BasicAck(ea.DeliveryTag, false);
            };
            //开始消费
            string actualConsumerTag = channel.BasicConsume(queue, false, consumer);
            _logger.LogInformation($"queue[{queue}] start consume. ActualConsumerTag: {actualConsumerTag}");
            //保持线程
            while (holdThread)
            {
                Thread.Sleep(10000);
            }
        }
        #endregion Methods
    }
}