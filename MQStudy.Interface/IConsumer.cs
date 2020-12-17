using System;
using System.Collections.Generic;
using System.Text;

namespace MQStudy.Interface
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
    }
}
