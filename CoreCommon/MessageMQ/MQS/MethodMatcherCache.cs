using CoreCommon.MessageMQ.Abstractions;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace CoreCommon.MessageMQ.MQS
{
    public class MethodMatcherCache
    {

        private ConcurrentDictionary<string, ConsumerExecutorDescriptor> Entries { get; }

        public MethodMatcherCache()
        {
            Entries = new ConcurrentDictionary<string, ConsumerExecutorDescriptor>();
        }

        /// <summary>
        /// Get a dictionary of candidates.In the dictionary,
        /// the Key is the CAPSubscribeAttribute Group, the Value for the current Group of candidates
        /// </summary>
        /// <param name="provider"><see cref="IServiceProvider"/></param>
        public ConcurrentDictionary<string, ConsumerExecutorDescriptor> GetCandidatesMethodsOfGroupNameGrouped(List<ConsumerExecutorDescriptor> queueNames)
        {
            if (Entries.Count != 0) return Entries;
            foreach (var item in queueNames)
            {
                Entries.TryAdd(item.Attribute.Name,item);
            }
            return Entries;
        }

        /// <summary>
        ///  Get a dictionary of specify topic candidates.
        ///  The Key is Group name, the value is specify topic candidates.
        /// </summary>
        /// <param name="topicName">message topic name</param>
        public ConsumerExecutorDescriptor GetTopicExector(string queueName)
        {
            if (Entries == null)
            {
                throw new ArgumentNullException(nameof(Entries));
            }

            ConsumerExecutorDescriptor groupd = null;
            Entries.TryGetValue(queueName, out groupd);
            return groupd;
        }
    }
}