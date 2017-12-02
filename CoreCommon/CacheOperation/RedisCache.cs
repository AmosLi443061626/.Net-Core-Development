using CoreCommon.Extensions;
using CoreCommon.RedisHelper;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoreCommon.CacheOperation
{
    public class RedisCache : ICache
    {
        public RedisCache()
        { }

        public string Get(string key, bool isDelay = false, int seconds = 30)
        {
            if (isDelay)
                StackExchangeRedisHelper.Instance().SetExpire(key, seconds);
            return StackExchangeRedisHelper.Instance().Get(key);
        }

        public T Get<T>(string key, bool isDelay = false, int seconds = 30)
        {
            if (isDelay)
                StackExchangeRedisHelper.Instance().SetExpire(key, seconds);
            return StackExchangeRedisHelper.Instance().Get<T>(key);
        }

        public bool Set<T>(string key, T value, int seconds = 30)
        {
            bool flag = StackExchangeRedisHelper.Instance().Set(key, value.ToJson());
            if (seconds > 0)
                StackExchangeRedisHelper.Instance().SetExpire(key, seconds);
            return flag;
        }

        public bool Set(string key, string value, int seconds = 30)
        {
            bool flag = StackExchangeRedisHelper.Instance().Set(key, value);
            if (seconds > 0)
                StackExchangeRedisHelper.Instance().SetExpire(key, seconds);
            return flag;
        }

        public bool Delete(string key)
        {
            StackExchangeRedisHelper.Instance().Remove(key);
            return true;
        }
    }
}
