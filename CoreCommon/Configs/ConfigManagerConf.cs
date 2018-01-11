using CoreCommon.CacheOperation;
using Microsoft.Extensions.Configuration;
using CoreCommon.Extensions;
using System.Collections.Concurrent;
using CoreCommon.RedisHelper;
using System.Collections.Generic;

namespace CoreCommon.Configs
{
    public static class ConfigManagerConf
    {
        public static IConfiguration Configuration = null;

        static ConcurrentDictionary<string, List<string>> _dicCache = new ConcurrentDictionary<string, List<string>>();

        static EtcdConfiWatcher etcdConfiWatcher;


        public static void SetConfiguration(IConfiguration configuration)
        {
            if (Configuration == null)
                Configuration = configuration;
            if (etcdConfiWatcher != null)
            {
                etcdConfiWatcher.Dispose();
            }
            if (!string.IsNullOrEmpty(GetValue("etcd:address")))
            {
                etcdConfiWatcher = new EtcdConfiWatcher(_dicCache);
            }
        }

        /// <summary>
        /// 值类型
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetValue(string key)
        {
            List<string> refValue;
            _dicCache.TryGetValue(key, out refValue);
            if (refValue != null)
                return refValue?[0];

            if (Configuration == null)
                return "";
            string value = Configuration[key];
            if (!value.IsNullOrEmpty()) //本地存在则返回
                return value;
            return "";
        }

        /// <summary>
        /// 引用类型(开发建议使用) List<string> list= ConfigManagerConf.GetReferenceValue("") 使用List 即可
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static List<string> GetReferenceValue(string key)
        {
            List<string> refValue;
            _dicCache.TryGetValue(key, out refValue);
            if (refValue == null)
            {
                refValue = new List<string>();
                refValue.Add(GetValue(key));//[0]
                _dicCache.TryAdd(key, refValue);
            }
            return refValue;
        }

        public static bool Set(string key, string value)
        {
            return etcdConfiWatcher.Set(key, value);
        }

        public static IDictionary<string, string> GetWatcherAll()
        {
            return etcdConfiWatcher.GetAll();
        }
    }
}
