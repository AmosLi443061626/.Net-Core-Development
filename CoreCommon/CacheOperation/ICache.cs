using System;
using System.Collections.Generic;
using System.Text;

namespace CoreCommon.CacheOperation
{
    public interface ICache
    {
         T Get<T>(string key, bool isDelay = false, int mins = 30);

        string Get(string key, bool isDelay = false, int mins = 30);

        bool Set<T>(string key, T value, int mins = 30);

        bool Set(string key, string value, int mins = 30);
    }
}
