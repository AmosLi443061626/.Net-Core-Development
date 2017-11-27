using System;
using CoreCommon.RequestExtend.Errors;

namespace CoreCommon.RequestExtend.Requester
{
    public class UnableToCompleteRequestError : Error
    {
        public UnableToCompleteRequestError(Exception exception) 
            : base($"Error making http request, exception: {exception.Message}",ErrorCode.UnableToCompleteRequestError)
        {
        }
    }
}
