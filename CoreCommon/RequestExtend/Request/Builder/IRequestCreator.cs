namespace CoreCommon.RequestExtend.Request.Builder
{
    using System.Net.Http;
    using System.Threading.Tasks;

    using CoreCommon.RequestExtend.Requester.QoS;
    using CoreCommon.RequestExtend.Responses;

    public interface IRequestCreator
    {
        Task<Response<Request>> Build(
            HttpRequestMessage httpRequestMessage,
            bool useCookieContainer,
            bool allowAutoRedirect);
    }
}
