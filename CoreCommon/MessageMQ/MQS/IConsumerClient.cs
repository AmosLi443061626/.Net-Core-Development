using System;
using System.Threading;
using CoreCommon.MessageMQ.Models;

namespace CoreCommon.MessageMQ.MQS
{
    /// <summary>
    /// consumer client
    /// </summary>
    public interface IConsumerClient : IDisposable
    {
        void Subscribe(string topic);

        void Subscribe(string topic,string group);
        void Subscribe(string topic, int partition);

        void Listening(TimeSpan timeout, CancellationToken cancellationToken);

        void Commit();

        event EventHandler<MessageContext> MessageReceieved;
    }
}