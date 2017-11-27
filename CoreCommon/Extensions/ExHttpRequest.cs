
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Text;

namespace CoreCommon.Extensions
{
    public static class ExHttpRequest
    {
        public static string ToReadAsyncJson(this HttpRequest request)
        {
            try
            {
                if (!(request.Body is MemoryStream))
                {
                    var stream = new MemoryStream();
                    request.Body.CopyTo(stream);
                    request.Body = stream;
                    stream.Position = 0;
                }
                var result = new StreamReader(request.Body).ReadToEnd();
                request.Body.Position = 0;
                return result;
            }
            catch (Exception)
            {
                return "";
            }
        }
    }
}
