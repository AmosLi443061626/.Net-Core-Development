﻿namespace CoreCommon.MessageMQ.MQS
{
    public class MessageContext
    {
        public string Group { get; set; }

        public string Name { get; set; }

        public string Content { get; set; }
    }
}