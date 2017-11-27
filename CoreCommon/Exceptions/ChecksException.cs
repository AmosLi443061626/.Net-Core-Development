using System;
using System.Collections.Generic;
using System.Text;

namespace CoreCommon.Exceptions
{
    /// <summary>
    /// 验证异常
    /// </summary>
    public class ChecksException : Exception
    {
        private int _code;
        private string _message;
        /// <summary>
        /// 异常消息
        /// </summary>
        public new string Message
        {
            get { return _message; }
            set { _message = value; }
        }

        public int Code
        {
            set { _code = value; }
            get { return _code; }
        }

        private ChecksException()
            : base()
        { }

        public ChecksException(string message)
            : base(message)
        {
            _message = message;
        }

        public ChecksException(string message, Exception innerException)
            : base(message, innerException)
        {
            _message = message;
        }
    }
}
