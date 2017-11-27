using System.Net.Http;
using System.Threading.Tasks;
using CoreCommon.RequestExtend.Responses;

namespace CoreCommon.RequestExtend.Requester
{
    public interface IHttpRequester
    {
        Task<Response<HttpResponseMessage>> GetResponse(Request.Request request);
    }
}
