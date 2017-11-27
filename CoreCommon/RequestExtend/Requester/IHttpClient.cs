using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace CoreCommon.RequestExtend.Requester
{
    public interface IHttpClient
    {
        HttpClient Client { get; }

        Task<HttpResponseMessage> SendAsync(HttpRequestMessage request);
    }
}
