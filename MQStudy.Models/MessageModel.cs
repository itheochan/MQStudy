using System;

namespace MQStudy.Models
{
    public class MessageModel
    {
        public string Exchange { get; set; }
        public string RoutingKey { get; set; }
        public string Body { get; set; }
    }
}
