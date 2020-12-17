/*
* --------------------------------------------------
* Copyright(C)		: 
* Namespace			: MQStudy.RMQ.Tests
* FileName			: Producer.cs
* CLR Version		: 4.0.30319.42000
* Author			: Theo
* CreateTime		: 2020/12/17 14:26:03
* --------------------------------------------------
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace MQStudy.RMQ.Tests
{
    ///<summary>
    /// Producer
    ///</summary>
    public class Producer
    {
        private IRabbitMQClient _RabbitMQClient;

        public Producer(IRabbitMQClient rabbitMQClient)
        {
            _RabbitMQClient = rabbitMQClient;
        }

        /// <summary>
        /// 基本模式：
        /// 生产者直接把消息发布到queue
        /// </summary>
        /// <returns></returns>
        public async Task PublishQueue()
        {
            await _RabbitMQClient.PublishAsync("Hello World!", "Q1");
        }

        /// <summary>
        /// 发布/订阅（扇出）模式:
        /// ExchangeType: fanout;
        /// 生产者发布消息到交换机，fanout类型的交换机把消息转发到与之绑定的所有队列中；
        /// 期间忽略routing key；
        /// 如果没有queue绑定到改queue，消息将被丢弃
        /// </summary>
        /// <returns></returns>
        public async Task PublishFanoutExchange()
        {
            await _RabbitMQClient.PublishAsync("fanout exchange", null, "amq.fanout");
        }

        /// <summary>
        /// 路由模式：
        /// ExchangeType: direct;
        /// 生产者发布消息到交换机，同时指定routing key，direct类型的交换机把消息转发给命中routing key的队列；
        /// 如果发布消息时不指定routing key，或者指定的routing key没有命中任何队列，消息将被丢弃
        /// </summary>
        /// <param name="routingKey"></param>
        /// <returns></returns>
        public async Task PublishDirectExchange(string routingKey)
        {
            await _RabbitMQClient.PublishAsync("direct exchange, routing key: " + routingKey, null, "amq.direct", routingKey);
        }

        /// <summary>
        /// 主题模式(匹配订阅)：
        /// ExchangeType: topic; 
        /// routing key规则：
        ///     ① "."分隔的单词列表，总长度不超过255bytes
        ///     ② 绑定的RoutingKey中，* 可以替代一个单词，# 可以替代0个或多个单词；
        /// 生产者发布消息到交换机，同时指定routing key，direct类型的交换机把消息转发给routing key相匹配的队列；
        /// 如果发布消息时不指定routing key，或者指定的routing key没有命中任何队列，消息将被丢弃
        /// </summary>
        /// <param name="routingKey"></param>
        /// <returns></returns>
        /// <remarks>
        /// 例如：
        ///     我们指定一个 topic 类型的交换机 cars
        ///     约定routing key 规则为 {type.year.color}: 类型.上市年份.颜色；
        ///     Q1 的routing key：*.2020.*
        ///     Q2 的routing key1：*.*.white
        ///     Q2 的routing key2：suv.#
        /// </remarks>
        public async Task PublishTopicExchange(string exchange, string routingKey)
        {
            await _RabbitMQClient.PublishAsync("topic exchange, routing key: " + routingKey, exchange, routingKey);
        }
    }
}