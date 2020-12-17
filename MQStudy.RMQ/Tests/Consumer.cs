/*
* --------------------------------------------------
* Copyright(C)		: 
* Namespace			: MQStudy.RMQ.Tests
* FileName			: Consumer.cs
* CLR Version		: 4.0.30319.42000
* Author			: Theo
* CreateTime		: 2020/12/17 15:32:31
* --------------------------------------------------
*/
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;

namespace MQStudy.RMQ.Tests
{
    ///<summary>
    /// Consumer
    ///</summary>
    public class Consumer
    {
        #region Fields
        private readonly ILogger<Consumer> _logger;
        private readonly RabbitMQClient _RabbitMQClient;
        #endregion Fields

        #region Ctor

        public Consumer(RabbitMQClient rabbitMQClient, ILogger<Consumer> logger)
        {
            _RabbitMQClient = rabbitMQClient;
            _logger = logger;
        }
        #endregion Ctor

        #region Properties
        #endregion Properties

        #region Methods
        public void Consume(string queue)
        {
            _RabbitMQClient.Consume(queue, (message) =>
            {
                _logger.LogInformation($"[{DateTimeOffset.Now:HH:mm:ss.fffff}] 队列 {queue} 收到消息：{message}");
                Thread.Sleep(100);
            });
        }
        #endregion Methods
    }
}