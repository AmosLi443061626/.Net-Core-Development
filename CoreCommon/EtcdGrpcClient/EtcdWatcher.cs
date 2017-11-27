using Etcdserverpb;
using Grpc.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using static Mvccpb.Event.Types;

namespace CoreCommon.EtcdGrcpClient
{
    public class EtcdWatchEvent
    {
        public string Key { get; }
        public string Value { get; }
        public EventType Type { get; }

        public EtcdWatchEvent(string key, string value, EventType type)
        {
            Key = key;
            Value = value;
            Type = type;
        }
    }

    public class EtcdWatcher : IDisposable
    {
        private AsyncDuplexStreamingCall<WatchRequest, WatchResponse> duplexCall;
        private List<Action<EtcdWatchEvent[]>> actions = new List<Action<EtcdWatchEvent[]>>();
        public bool isDisposed = false;

        public EtcdWatcher(AsyncDuplexStreamingCall<WatchRequest, WatchResponse> duplexCall)
        {
            this.duplexCall = duplexCall;
            Watch(duplexCall.ResponseStream);
        }

        private async void Watch(IAsyncStreamReader<WatchResponse> responseStream)
        {
            do
            {
                try
                {
                    await responseStream.MoveNext();
                }
                catch (Exception)
                {
                    Dispose();
                    break;
                }

                var watchEvents = responseStream.Current.Events.Select(ev =>
                    {
                        var key = ev.Kv.Key.ToStringUtf8();
                        var value = ev.Kv.Value.ToStringUtf8();
                        var type = ev.Type;
                        return new EtcdWatchEvent(key, value, type);
                    }
                ).ToArray();
                if (watchEvents.Length != 0)
                {
                    actions.ForEach(a => a(watchEvents));
                }
            } while (!isDisposed && !responseStream.Current.Canceled);
        }

        public void Subscribe(Action<EtcdWatchEvent[]> action)
        {
            actions.Add(action);
        }

        public void Dispose()
        {
            isDisposed = true;
            this.duplexCall.Dispose();
        }
    }
}
