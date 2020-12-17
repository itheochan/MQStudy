using System;
using System.Collections.Generic;
using System.Text;

namespace MQStudy.RMQ
{
    public interface IConsumer
    {
        /// <summary>
        /// Consume
        /// </summary>
        /// <param name="queue">队列</param>
        /// <param name="handler">消费委托</param>
        /// <param name="holdThread">是否保持线程, 默认true</param>
        void Consume(string queue, Action<string> handler, bool holdThread = true);


        /// <summary>
        /// 并发消费
        /// </summary>
        /// <param name="queue">队列</param>
        /// <param name="handler">消费委托</param>
        /// <param name="consumerDispatchConcurrency">消费者并发数</param>
        /// <param name="holdThread">是否保持线程, 默认true</param>
        void Consume(string queue, Action<string> handler, int consumerDispatchConcurrency, bool holdThread = true);
    }
}
