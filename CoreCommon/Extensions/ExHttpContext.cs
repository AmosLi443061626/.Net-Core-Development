using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;

namespace CoreCommon.Extensions
{
    public static class HttpContextExtension
    {
        public static string GetUserIp(this HttpContext context)
        {
            var ip = context.Request.Headers["X-Forwarded-For"].FirstOrDefault();
            if (string.IsNullOrEmpty(ip))
            {
                ip = context.Connection.RemoteIpAddress.ToString();
            }
            return ip;
        }

        public static SortedDictionary<string, string> GetRequestPost(this HttpContext httpContext)
        {
            SortedDictionary<string, string> sArray = new SortedDictionary<string, string>();
            try
            {
                if (httpContext.Request.Form == null) return sArray;
            }
            catch { return sArray; }
            var req = httpContext.Request.Form.Keys.ToList();
            for (int i = 0; i < req.Count; i++)
            {
                sArray.Add(req[i], httpContext.Request.Form[req[i]]);
            }
            return sArray;
        }
    }
}
