/*
* --------------------------------------------------
* Copyright(C)		: 
* Namespace			: MQStudy.RMQ
* FileName			: RabbitMQOptions.cs
* CLR Version		: 4.0.30319.42000
* Author			: Theo
* CreateTime		: 2020/12/17 10:52:25
* --------------------------------------------------
*/
using System.Collections.Generic;

namespace MQStudy.RMQ
{
    ///<summary>
    /// RabbitMQOptions
    ///</summary>
    public class RabbitMQOptions
    {
        public const string Position = "RabbitMQ";
        public string UserName { get; set; }
        public string Password { get; set; }
        public string HostName { get; set; }
        /// <summary>
        /// 集群模式下的host列表
        /// </summary>
        public IList<string> HostNames { get; set; }
        /// <summary>
        /// 集群模式下的(host,port)列表
        /// </summary>
        public Dictionary<string,int> Endpoints { get; set; }
        public int Port { get; set; }
        public string VirtualHost { get; set; }
        public string ClientProvidedName { get; set; }
        public int ConsumerDispatchConcurrency { get; set; }

        /// <summary>
        /// 预留，可能会用于身份验证
        /// </summary>
        public int Type { get; set; }
    }
}