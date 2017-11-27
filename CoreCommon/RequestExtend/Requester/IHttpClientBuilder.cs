using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using CoreCommon.RequestExtend.Requester.QoS;
using System.Net;

namespace CoreCommon.RequestExtend.Requester
{
    public interface IHttpClientBuilder
    {

        /// <summary>
        /// Sets a PollyCircuitBreakingDelegatingHandler .
        /// </summary>
        IHttpClientBuilder WithQos();

        /// <summary>
        /// Creates the <see cref="HttpClient"/>
        /// </summary>
        /// <param name="useCookies">Defines if http client should use cookie container</param>
        /// <param name="allowAutoRedirect">Defines if http client should allow auto redirect</param>
        IHttpClient Create(bool useCookies, bool allowAutoRedirect);
    }
}
