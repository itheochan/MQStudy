/*
* --------------------------------------------------
* Copyright(C)		: 
* Namespace			: MQStudy.Worker
* FileName			: RabbitMQConsumer.cs
* CLR Version		: 4.0.30319.42000
* Author			: Theo
* CreateTime		: 2020/12/17 16:31:03
* --------------------------------------------------
*/
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MQStudy.RMQ;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace MQStudy.Worker
{
    ///<summary>
    /// RabbitMQConsumer
    ///</summary>
    public class RabbitMQConsumer : BackgroundService
    {
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var client = Program.GetService<IRabbitMQClient>();
            var logger = Program.GetService<ILogger<RabbitMQConsumer>>();
            var configuration = Program.GetService<IConfiguration>();
            string queue = configuration[RabbitMQOptions.Position + ":Queue"];
            logger.LogInformation($"队列 {queue} 开始消费");
            client.Consume(queue, (message) =>
            {
                logger.LogInformation($"[{DateTimeOffset.Now:HH:mm:ss.fffff}] 队列 {queue} 收到消息：{message}");
                Thread.Sleep(500);
            });
            logger.LogInformation($"队列 {queue} 结束消费");
            return Task.CompletedTask;
        }
    }
}