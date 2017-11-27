using Etcdserverpb;
using Grpc.Core;
using System;
using System.Threading.Tasks;

namespace CoreCommon.EtcdGrcpClient
{
    public class EtcdLeaseTTLRefresher : IDisposable
    {
        private readonly AsyncDuplexStreamingCall<LeaseKeepAliveRequest, LeaseKeepAliveResponse> duplexCall;

        public EtcdLeaseTTLRefresher(AsyncDuplexStreamingCall<LeaseKeepAliveRequest, LeaseKeepAliveResponse> duplexCall)
        {
            this.duplexCall = duplexCall;
        }

        public void Dispose()
        {
            duplexCall.Dispose();
        }

        public Task RefreshLease(long leaseId)
        {
            return duplexCall.RequestStream.WriteAsync(new LeaseKeepAliveRequest() { ID = leaseId });
        }
    }
}
