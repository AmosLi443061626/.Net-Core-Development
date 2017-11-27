using System;
using System.Collections.Generic;
using System.Net.Http;
using Polly;
using Polly.CircuitBreaker;
using Polly.Timeout;

namespace Ocelot.Requester.QoS
{
    public interface IQoSProvider
    {
        CircuitBreaker CircuitBreaker { get; }
    }

    public class NoQoSProvider : IQoSProvider
    {
        public CircuitBreaker CircuitBreaker { get; }
    }

    public class PollyQoSProvider : IQoSProvider
    {
        private readonly CircuitBreakerPolicy _circuitBreakerPolicy;
        private readonly TimeoutPolicy _timeoutPolicy;
        private readonly CircuitBreaker _circuitBreaker;

        public PollyQoSProvider()
        {

            _timeoutPolicy = Policy.TimeoutAsync(TimeSpan.FromMilliseconds(20000));

            _circuitBreakerPolicy = Policy
                .Handle<HttpRequestException>()
                .Or<TimeoutRejectedException>()
                .Or<TimeoutException>()
                .CircuitBreakerAsync(
                    exceptionsAllowedBeforeBreaking: 1,
                    durationOfBreak: TimeSpan.FromMilliseconds(2000),
                    onBreak: (ex, breakDelay) =>
                    {

                    },
                    onReset: () =>
                    {

                    },
                    onHalfOpen: () =>
                    {
                    }
                );

            _circuitBreaker = new CircuitBreaker(_circuitBreakerPolicy, _timeoutPolicy);
        }

        public CircuitBreaker CircuitBreaker => _circuitBreaker;
    }

    public class CircuitBreaker
    {
        public CircuitBreaker(CircuitBreakerPolicy circuitBreakerPolicy, TimeoutPolicy timeoutPolicy)
        {
            CircuitBreakerPolicy = circuitBreakerPolicy;
            TimeoutPolicy = timeoutPolicy;
        }

        public CircuitBreakerPolicy CircuitBreakerPolicy { get; private set; }
        public TimeoutPolicy TimeoutPolicy { get; private set; }
    }

}
