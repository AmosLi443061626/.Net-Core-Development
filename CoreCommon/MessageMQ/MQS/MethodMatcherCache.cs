using CoreCommon.MessageMQ.Abstractions;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace CoreCommon.MessageMQ.MQS
{
    public class MethodMatcherCache
    {

        private ConcurrentDictionary<string, IList<ConsumerExecutorDescriptor>> Entries { get; }

        public MethodMatcherCache()
        {
            Entries = new ConcurrentDictionary<string, IList<ConsumerExecutorDescriptor>>();
        }

        /// <summary>
        /// Get a dictionary of candidates.In the dictionary,
        /// the Key is the CAPSubscribeAttribute Group, the Value for the current Group of candidates
        /// </summary>
        /// <param name="provider"><see cref="IServiceProvider"/></param>
        public ConcurrentDictionary<string, IList<ConsumerExecutorDescriptor>> GetCandidatesMethodsOfGroupNameGrouped(IList<ConsumerExecutorDescriptor> grouped)
        {
            if (Entries.Count != 0) return Entries;

            IList<ConsumerExecutorDescriptor> groupd = null;

            foreach (var item in grouped)
            {
                Entries.TryGetValue(item.Attribute.Group, out groupd);
                if (groupd==null)
                {
                    groupd = new List<ConsumerExecutorDescriptor>();
                }
                groupd.Add(item);
                Entries.TryAdd(item.Attribute.Group, groupd);
            }
            return Entries;
        }

        /// <summary>
        ///  Get a dictionary of specify topic candidates.
        ///  The Key is Group name, the value is specify topic candidates.
        /// </summary>
        /// <param name="topicName">message topic name</param>
        public IDictionary<string, IList<ConsumerExecutorDescriptor>> GetTopicExector(string topicName)
        {
            if (Entries == null)
            {
                throw new ArgumentNullException(nameof(Entries));
            }

            var dic = new Dictionary<string, IList<ConsumerExecutorDescriptor>>();
            foreach (var item in Entries)
            {
                var topicCandidates = item.Value.Where(x => x.Attribute.Name == topicName);
                dic.Add(item.Key, topicCandidates.ToList());
            }
            return dic;
        }
    }
}