namespace CoreCommon.RequestExtend.Responses
{
    public class OkResponse<T> : Response<T>
    {
        public OkResponse(T data) : base(data)
        {
        }
    }
}