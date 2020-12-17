using System;
using System.Collections.Generic;
using System.Text;

namespace MQStudy.Interface
{
    /// <summary>
    /// 消息队列客户端
    /// </summary>
    public interface IMQClient : IProducer, IConsumer
    {

    }
}
