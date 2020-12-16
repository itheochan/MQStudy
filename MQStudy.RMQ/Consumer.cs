/*
* --------------------------------------------------
* Copyright(C)		: 
* Namespace			: MQStudy.RMQ
* FileName			: Consumer.cs
* CLR Version		: 4.0.30319.42000
* Author			: iTheo
* CreateTime		: 2020/12/6 21:06:48
* --------------------------------------------------
*/

using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
public class Consumer
{
    public void Consume()
    {
        var factory = new ConnectionFactory() { HostName = "localhost", UserName = "admin", Password = "admin", VirtualHost = "/" };
        using var connection = factory.CreateConnection();
        using var channel = connection.CreateModel();
        channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);
        channel.QueueDeclare(queue: "hello", durable: false, exclusive: false, autoDelete: false, arguments: null);
        var consumer = new EventingBasicConsumer(channel);
        consumer.Received += (sender, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            Console.WriteLine(" [x] Received {0}", message);
            // 应答服务器，告知已经处理完毕，可以接收下一条消息
            channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
        };
        channel.BasicConsume(queue: "hello",
                             autoAck: false,    //设置为主动应答
                             consumer: consumer);
        //while (true)
        //{
        //    System.Threading.Thread.Sleep(1000);
        //}
    }
}
