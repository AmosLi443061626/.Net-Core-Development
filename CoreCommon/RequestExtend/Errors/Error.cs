namespace CoreCommon.RequestExtend.Errors
{
    public abstract class Error 
    {
        protected Error(string message, ErrorCode code)
        {
            Message = message;
            Code = code;
        }

        public string Message { get; private set; }
        public ErrorCode Code { get; private set; }
    }
}