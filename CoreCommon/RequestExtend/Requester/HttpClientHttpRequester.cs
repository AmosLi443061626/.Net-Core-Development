using System;
using System.Collections.Concurrent;
using System.Net.Http;
using System.Threading.Tasks;
using Polly.CircuitBreaker;
using Polly.Timeout;
using CoreCommon.RequestExtend.Responses;

namespace CoreCommon.RequestExtend.Requester
{
    public class HttpClientHttpRequester : IHttpRequester
    {
        private readonly IHttpClientCache _cacheHandlers;

        public HttpClientHttpRequester(IHttpClientCache cacheHandlers)
        {
            _cacheHandlers = cacheHandlers;
        }

        public async Task<Response<HttpResponseMessage>> GetResponse(Request.Request request)
        {
            var builder = new HttpClientBuilder();

            var cacheKey = GetCacheKey(request, builder);
            
            var httpClient = GetHttpClient(cacheKey, builder, request.UseCookieContainer, request.AllowAutoRedirect);

            try
            {
                var response = await httpClient.SendAsync(request.HttpRequestMessage);
                return new OkResponse<HttpResponseMessage>(response);
            }
            catch (TimeoutRejectedException exception)
            {
                return
                    new ErrorResponse<HttpResponseMessage>(new RequestTimedOutError(exception));
            }
            catch (BrokenCircuitException exception)
            {
                return
                    new ErrorResponse<HttpResponseMessage>(new RequestTimedOutError(exception));
            }
            catch (Exception exception)
            {
                return new ErrorResponse<HttpResponseMessage>(new UnableToCompleteRequestError(exception));
            }
            finally
            {
                _cacheHandlers.Set(cacheKey, httpClient, TimeSpan.FromHours(24));
            }

        }

        private IHttpClient GetHttpClient(string cacheKey, IHttpClientBuilder builder, bool useCookieContainer, bool allowAutoRedirect)
        {
            var httpClient = _cacheHandlers.Get(cacheKey);

            if (httpClient == null)
            {
                httpClient = builder.Create(useCookieContainer, allowAutoRedirect);
            }
            return httpClient;
        }

        private string GetCacheKey(Request.Request request, IHttpClientBuilder builder)
        {
            string baseUrl = $"{request.HttpRequestMessage.RequestUri.Scheme}://{request.HttpRequestMessage.RequestUri.Authority}";
            builder.WithQos();
            return baseUrl;
        }
    }
}