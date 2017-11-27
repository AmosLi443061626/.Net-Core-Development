﻿namespace CoreCommon.MessageMQ.MQS
{
    /// <summary>
    /// Consumer client factory to create consumer client instance.
    /// </summary>
    public interface IConsumerClientFactory
    {
        /// <summary>
        /// Create a new instance of <see cref="IConsumerClient"/>.
        /// </summary>
        /// <param name="groupId"></param>
        IConsumerClient Create(string groupId);
    }
}