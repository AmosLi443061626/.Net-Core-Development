
using CoreCommon.EtcdGrcpClient;
using Etcdserverpb;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CoreCommon.Configs
{
    /// <summary>
    /// Etcd 配置文件监控
    /// </summary>
    public class EtcdConfiWatcher : IDisposable
    {
        EtcdClient etcdClient;
        EtcdWatcher watcher;
        readonly string conf = ConfigManagerConf.GetValue("etcd:node");
        private int isTaked = 0;

        public EtcdConfiWatcher(ConcurrentDictionary<string, List<string>> cd)
        {
            etcdClient = new EtcdClient(new Uri(ConfigManagerConf.GetValue("etcd:address")));
            WaitWatch(cd);
        }

        public async void WaitWatch(ConcurrentDictionary<string, List<string>> cd)
        {
            while (Interlocked.Exchange(ref isTaked, 0) != 1)
            {
                if (watcher == null || watcher.isDisposed)
                {
                    try
                    {
                        WatchValues(cd);
                    }
                    catch { }
                }
                await Task.Delay(new Random().Next(2000, 15000));
            }
        }

        private void
WatchValues(ConcurrentDictionary<string, List<string>> cd)
        {
            try
            {
                var dk = etcdClient.GetRange(conf).Result;
                foreach (var item in dk)
                    cd.AddOrUpdate(item.Key.Replace(conf, ""), new List<string> { item.Value }, (key, value) =>
                    {
                        value[0] = item.Value;
                        return value;
                    });
            }
            catch
            {
                foreach (var item in cd)
                     etcdClient.Put(conf + item.Key, item.Value[0]).Wait();
            }
            watcher = etcdClient.WatchRange(conf).Result;
            watcher.Subscribe(s =>
            {
                foreach (var item in s)
                    cd.AddOrUpdate(item.Key.Replace(conf, ""), new List<string> { item.Value }, (key, value) =>
                    {
                        value[0] = item.Value;
                        return value;
                    });
            });
        }

        public IDictionary<string, string> GetAll()
        {
            return etcdClient.GetRange("/").Result;
        }

        public void Dispose()
        {
            Interlocked.Exchange(ref isTaked, 1);
            if (watcher != null)
                watcher.Dispose();
            if (etcdClient != null)
                etcdClient.Dispose();
            GC.SuppressFinalize(true);
        }
    }
}
