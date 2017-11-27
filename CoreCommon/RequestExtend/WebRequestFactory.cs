using CoreCommon.RequestExtend.Requester;
using CoreCommon.RequestExtend.Responses;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace CoreCommon.RequestExtend
{
    public static class WebRequestFactory
    {
        private static IHttpRequester _requester;

        private static IHttpClientCache _clientCache;

        static WebRequestFactory()
        {
            _clientCache = new MemoryHttpClientCache();
            _requester = new HttpClientHttpRequester(_clientCache);
        }

        public static Response<HttpResponseMessage> GetResponse(Request.Request request)
        {
            return _requester.GetResponse(request).Result;
        }
    }
}
