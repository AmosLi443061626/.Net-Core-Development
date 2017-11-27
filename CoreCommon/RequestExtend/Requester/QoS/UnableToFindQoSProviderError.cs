
using CoreCommon.RequestExtend.Errors;

namespace CoreCommon.RequestExtend.Requester.QoS
{
    public class UnableToFindQoSProviderError : Error
    {
        public UnableToFindQoSProviderError(string message) 
            : base(message, CoreCommon.RequestExtend.Errors.ErrorCode.UnableToFindQoSProviderError)
        {
        }
    }
}
