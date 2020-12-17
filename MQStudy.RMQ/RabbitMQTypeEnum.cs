/*
* --------------------------------------------------
* Copyright(C)		: 
* Namespace			: MQStudy.RMQ
* FileName			: RabbitMQ.cs
* CLR Version		: 4.0.30319.42000
* Author			: Theo
* CreateTime		: 2020/12/17 10:53:09
* --------------------------------------------------
*/
using System.ComponentModel;

namespace MQStudy.RMQ
{
    ///<summary>
    /// RabbitMQ类型，用于身份验证
    ///</summary>
    public enum RabbitMQTypeEnum
    {
        /// <summary>
        /// RabbitMQ
        /// </summary>
        [Description("RabbitMQ")]
        RabbitMQ = 0,
        /// <summary>
        /// 阿里云AMQP
        /// </summary>
        [Description("AliyunAMQP")]
        AliyunAMQP = 1
    }
}