using System;

namespace CoreCommon.Exceptions
{
    public class QueueException : Exception
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

        private QueueException()
            : base()
        { }

        public QueueException(string message)
            : base(message)
        {
            _message = message;
        }

        public QueueException(string message, Exception innerException)
            : base(message, innerException)
        {
            _message = message;
        }
    }
}
