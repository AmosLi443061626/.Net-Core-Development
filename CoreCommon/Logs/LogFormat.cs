using CoreCommon.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoreCommon.Logs
{
    /// <summary>
    /// Json格式
    /// </summary>
    public class LogFormat
    {
        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="level">错误等级</param>
        /// <param name="msg">简述</param>
        /// <param name="app">项目名称</param>
        /// <param name="group">日志一级分类</param>
        /// <param name="obj">日志二级分类</param>
        /// <param name="status">状态码</param>
        /// <param name="elapsed">执行时间毫秒</param>
        /// <param name="error">错误完整信息</param>
        /// <param name="url">url</param>
        /// <param name="route">路由信息</param>
        public LogFormat(string msg, string app, string group, string obj, int status, int elapsed, string error, string url, string route)
        {
            this.sid = Guid.NewGuid().ToString();
            this.time = DateTime.Now;
            this.level = level;
            this.msg = msg;
            this.app = app;
            this.group = group;
            this.obj = obj;
            this.status = status;
            this.elapsed = elapsed;
            this.error = error;
            this.url = url;
            this.route = route;
        }

        /// <summary>
        /// guid/uuid
        /// </summary>
        public string sid { get; set; }
        /// <summary>
        /// log生成时间
        /// </summary>
        public DateTime time { get; set; }
        /// <summary>
        /// log等级
        /// </summary>
        public string level { get; set; }
        /// <summary>
        /// 信息简要
        /// </summary>
        public string msg { get; set; }
        /// <summary>
        /// 项目名称
        /// </summary>
        public string app { get; set; }
        /// <summary>
        /// 日志一级分类
        /// </summary>
        public string group { get; set; }
        /// <summary>
        /// 日志二级分类
        /// </summary>
        public string obj { get; set; }
        /// <summary>
        /// 状态码
        /// </summary>
        public int status { get; set; }
        /// <summary>
        /// 执行毫秒
        /// </summary>
        public int elapsed { get; set; }
        /// <summary>
        /// 错误的完整信息
        /// </summary>
        public string error { get; set; }
        public string url { get; set; }
        public string route { get; set; }
    }
}
