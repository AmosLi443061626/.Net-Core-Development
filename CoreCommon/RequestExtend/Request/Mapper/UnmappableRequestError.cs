namespace CoreCommon.RequestExtend.Request.Mapper
{
    using CoreCommon.RequestExtend.Errors;
    using System;

    public class UnmappableRequestError : Error
    {
        public UnmappableRequestError(Exception ex) : base($"Error when parsing incoming request, exception: {ex.Message}", ErrorCode.UnmappableRequestError)
        {
        }
    }
}
