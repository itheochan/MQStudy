using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MQStudy.RMQ
{
    public interface IProducer
    {

        /// <summary>
        /// 发布消息
        /// </summary>
        /// <param name="message">消息内容</param>
        /// <param name="exchange">交换机</param>
        /// <param name="queue">队列</param>
        /// <param name="routingKey">路由</param>
        /// <returns></returns>
        void Publish(string message, string queue, string exchange = "", string routingKey = null);

        /// <summary>
        /// 异步发布消息
        /// </summary>
        /// <param name="message">消息内容</param>
        /// <param name="exchange">交换机</param>
        /// <param name="queue">队列</param>
        /// <param name="routingKey">路由</param>
        /// <returns></returns>
        Task PublishAsync(string message, string queue, string exchange = "", string routingKey = null);
    }
}
