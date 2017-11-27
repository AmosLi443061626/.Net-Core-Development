using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreCommon.Results
{
    public class Result
    {
        public Boolean success { get; set; }

        public String msg { get; set; }

        public int code { get; set; }

        public object data { get; set; }

        public Result(bool success, string msg, int code, object data)
        {
            this.success = success;
            this.msg = msg;
            this.code = code;
            this.data = data;
        }


        public Result()
        {
        }

        public static Result Fail(int code, String msg = "")
        {
            return new Result
            {
                success = false,
                msg = msg,
                code = code
            };
        }

        public static Result Success(String msg)
        {
            return new Result
            {
                success = true,
                msg = msg
            };
        }

        public static Result Success()
        {
            return Success(string.Empty);
        }


        public static Result<T> Success<T>(T data)
        {
            return new Result<T>
            {
                success = true,
                Data = data
            };
        }

        public static Result<T> Fail<T>(int code, T data, String msg = "")
        {
            return new Result<T>
            {
                success = false,
                msg = msg,
                code = code,
                Data = data
            };
        }
    }

    /// <summary>
    /// 返回参数定义
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Result<T> : Result
    {
        /// <summary>数据
        /// </summary>
        public T Data { get; set; }

        public Result() { }

        public Result(bool success,string msg,int code,T data)
        {
            this.success = success;
            this.msg = msg;
            this.code = code;
            this.Data = data;
        }

        /// <summary>失败
        /// </summary>
        /// <param name="msg">定义失败信息</param>
        /// <returns></returns>
        public static Result<T> Fail(String msg, int code)
        {
            return new Result<T>
            {
                success = false,
                msg = msg,
                code = code
            };
        }
        /// <summary>失败 
        /// </summary>
        /// <param name="data">要返回的数据泛型</param>
        /// <param name="code">错误code</param>
        /// <param name="msg">定义失败信息</param>
        /// <returns></returns>
        public static Result<T> Fail(T data, int code, String msg = "")
        {
            return new Result<T>
            {
                success = false,
                msg = msg,
                Data = data,
                code = code
            };
        }

        /// <summary>成功 
        /// </summary>
        /// <param name="data">要返回的数据泛型</param>
        /// <param name="msg">可定义成功信息</param>
        /// <returns></returns>
        public static Result<T> Success(T data, String msg)
        {
            return new Result<T>
            {
                success = true,
                msg = msg,
                Data = data
            };
        }

        /// <summary>成功 
        /// </summary>
        /// <param name="data">要返回的数据泛型</param>
        /// <returns></returns>
        public static Result<T> Success(T data)
        {
            return Success(data, string.Empty);
        }
    }
}
