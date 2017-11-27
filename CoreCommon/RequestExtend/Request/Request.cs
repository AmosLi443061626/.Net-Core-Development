using System.Net.Http;
using CoreCommon.RequestExtend.Requester.QoS;

namespace CoreCommon.RequestExtend.Request
{
    public class Request
    {
        public Request(HttpRequestMessage httpRequestMessage, bool allowAutoRedirect,
            bool useCookieContainer)
        {
            HttpRequestMessage = httpRequestMessage;
            AllowAutoRedirect = allowAutoRedirect;
            UseCookieContainer = useCookieContainer;
        }

        public HttpRequestMessage HttpRequestMessage { get; private set; }
        public bool AllowAutoRedirect { get; private set; }
        public bool UseCookieContainer { get; private set; }
    }
}
