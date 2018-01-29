using System;

namespace CoreCommon.MessageMQ.Models
{
    public class PublishedMessage
    {
        /// <summary>
        /// Initializes a new instance of <see cref="CapPublishedMessage"/>.
        /// </summary>
        /// <remarks>
        /// The Id property is initialized to from a new GUID string value.
        /// </remarks>
        public PublishedMessage()
        {
            time = DateTime.Now;
        }

        /// <summary>
        /// 信息唯一Id
        /// </summary>
        public string rid { get; set; }
        /// <summary>
        /// 区域名称
        /// </summary>
        public string exchangeName { get; set; }
        /// <summary>
        /// 队列名称
        /// </summary>
        public string queueName { get; set; }
        /// <summary>
        /// 消息体Json
        /// </summary>
        public string msgBeanJson { get; set; }
        /// <summary>
        /// 消费次数
        /// </summary>
        public int consumerNum { get; set; } = 0;
        /// <summary>
        /// 最大重试次数
        /// </summary>
        public int maxNum { get; set; }
        /// <summary>
        /// 过期时间
        /// </summary>
        public DateTime timeout { get; set; }
        /// <summary>
        /// 消息时间
        /// </summary>
        public DateTime time { get; set; }
    }
}