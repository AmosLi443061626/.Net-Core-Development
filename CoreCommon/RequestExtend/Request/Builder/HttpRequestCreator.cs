using System.Threading.Tasks;
using CoreCommon.RequestExtend.Responses;
using CoreCommon.RequestExtend.Requester.QoS;
using System.Net.Http;

namespace CoreCommon.RequestExtend.Request.Builder
{
    public sealed class HttpRequestCreator : IRequestCreator
    {
        public async Task<Response<Request>> Build(
            HttpRequestMessage httpRequestMessage,
            bool useCookieContainer,
            bool allowAutoRedirect)
        {
            return new OkResponse<Request>(new Request(httpRequestMessage, useCookieContainer, allowAutoRedirect));
        }
    }
}